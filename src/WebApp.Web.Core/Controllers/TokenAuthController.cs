using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using WebApp.WebCore.Authentication.External;
using WebApp.WebCore.Models.TokenAuth;
using WebApp.WebCore.Authentication.BaseManager;
using WebApp.WebCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Abp.Auditing;
using WebApp.Application;
using Abp.Runtime.Caching;
using WebApp.WebCore.Models.Account;
using Microsoft.AspNetCore.Http;
using WebApp.WebCore.Models;
using Microsoft.AspNetCore.Cors;

namespace WebApp.WebCore.Controllers
{
    
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : WebAppControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly LogInManager _logInManager;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        //private readonly ISession _session;

        public TokenAuthController(
            LogInManager logInManager,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
             ICacheManager cacheManager)
        {
            _logInManager = logInManager;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _cacheManager = cacheManager;
        }

        //从缓存中获取token（只有45秒缓存）
        [AllowAnonymous]
        [HttpGet]
        [EnableCors("localhost")]
        public async Task<AuthenticateResultModel> GetTokenFromCache()
        {
            AuthenticateResultModel output;
            try
            {
                //string serverState = _session.GetString("serverState");
                string serverState = HttpContext.Request.Query["serverState"].FirstOrDefault() ?? "";
                string clientState = HttpContext.Request.Query["state"].FirstOrDefault() ?? "";
                string clientId = HttpContext.Request.Query["clientId"].FirstOrDefault() ?? "";

                string ticket = Common.Common.MD5String("utf-8", clientId + clientState + serverState).ToLower();

                var cachemodel = await _cacheManager.GetCache("LoginTokenCache").GetAsync(ticket, null);
                output = cachemodel as AuthenticateResultModel;
            }
            catch
            {
                throw new UserFriendlyException("登录异常", "所请求的访问token不存在");
            }
            return output;
        }


        /// <summary>
        /// 返回ticket，缓存token
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> GetTicket([FromBody] LoginFromBodyModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            AuthenticateResultModel authTokenModel = new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };

            string serverState = System.Guid.NewGuid().ToString("N");
            //if(!string.IsNullOrEmpty(_session.GetString("serverState")))
            //{
            //    _session.Remove("serverState");
            //}
            //_session.SetString("serverState", serverState);

            string ticket = Common.Common.MD5String("utf-8", model.ClientId + model.ClientState + serverState).ToLower();

            var expire = TimeSpan.FromSeconds(45);
            _cacheManager.GetCache("LoginTokenCache").DefaultSlidingExpireTime = expire;
            _cacheManager.GetCache("LoginTokenCache").Set(ticket, authTokenModel, expire);

            return serverState;
        }


        //[DisableAuditing]
        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model, string returnUrl = "")
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));



            //if (string.IsNullOrWhiteSpace(returnUrl))
            //{
            //    returnUrl = Request.ApplicationPath + "/Home/Index";
            //}
            //return Json(new MvcAjaxResponse { TargetUrl = returnUrl });

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };

            //return new AuthenticateResultModel
            //{
            //    AccessToken = model.UserNameOrEmailAddress,
            //    EncryptedAccessToken = model.Password,
            //    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            //};
        }


        private async Task<LoginResult> GetLoginResultAsync(string usernameOrEmailAddress, string password)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password);

            switch (loginResult.Result)
            {
                case LoginResultType.Success:
                    return loginResult;
                case LoginResultType.InvalidUserNameOrEmailAddress:
                    throw new UserFriendlyException("登录失败","无效的登录用户名");
                case LoginResultType.InvalidPassword:
                    throw new UserFriendlyException("登录失败","无效的登录密码");
                case LoginResultType.LockedOut:
                    throw new UserFriendlyException("登录失败",string.Format("用户 {0} 未激活，不能登录", loginResult.User.UserAccout));
                default:
                    throw new UserFriendlyException("登录失败","用户名或密码无效");
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}
