using Abp.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.WebCore.Authentication.JwtBearer
{
    /// <summary>
    /// 权限中间件
    /// </summary>
    public class PermissionMiddleware
    {
        /// <summary>
        /// 管道代理对象
        /// </summary>
        private readonly RequestDelegate _next;



        /// <summary>
        /// 用户权限集合
        /// </summary>
        //internal static List<UserPermission> _userPermissions;

        /// <summary>
        /// 权限中间件构造
        /// </summary>
        /// <param name="next">管道代理对象</param>
        /// <param name="permissionResitory">权限仓储对象</param>
        /// <param name="option">权限中间件配置选项</param>
        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 调用管道
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {

            //请求Url
            var questUrl = context.Request.Path.Value.ToLower();

            //是否经过验证
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                if (true)
                //if (_userPermissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == questUrl).Count() > 0)
                {
                    //用户名
                    //var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                    if (true)
                    //if (_userPermissions.Where(w => w.UserName == userName && w.Url.ToLower() == questUrl).Count() > 0)

                    {
                        return this._next(context);
                    }
                    //else
                    //{
                    //    //无权限跳转到拒绝页面
                    //    context.Response.Redirect("/");
                    //}
                }
            }
            else
            {
                //string path = context.Request.Path;
                //string strUrl = "/Account/LogOn?returnUrl={0}";

                //context.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path));
                //context.Response.Redirect("/");

                //context.Result = new ContentResult { Content = @"JsHelper.ShowError('Sorry,you have no authorization to do this action！')" };
                //var result = new ResultModel(Code.Unauthorized, "没有足够的权限操作资源");
                //context.Response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(result.ToJson()) };
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                //var content = new Result
                //{
                //   success = false,
                //    errs = new[] { "服务端拒绝访问：你没有权限，或者掉线了" }
                // };
                //response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");

                //context.HttpContext.Response.StatusCode = GetStatusCode(context);

                //throw new AbpAuthorizationException("没有足够的权限操作资源");
                //context.Response.en

                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Status = false,
                    Message = "认证失败"
                }));

            }
            return this._next(context);
            //
        }
    }
}