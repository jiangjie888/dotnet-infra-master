using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Timing;
using WebApp.Core.SysUserInfos;
using Abp.Auditing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApp.WebCore.Authentication.BaseManager
{
    public class LogInManager : ITransientDependency
    {


        private readonly IRepository<SysUserInfo, long> _userRepository;
        private IRepository<SysUserLoginAtt, long> _userLoginAttRepository { get; }

        //protected ISettingManager SettingManager { get; }

        //private readonly IPasswordHasher<TUser> _passwordHasher;
        private readonly IUserClaimsPrincipalFactory<SysUserInfo> _claimsPrincipalFactory;


        private IUnitOfWorkManager UnitOfWorkManager { get; }
        public IClientInfoProvider ClientInfoProvider { get; set; }
        //private IIocResolver IocResolver { get; }

        public LogInManager(
            IRepository<SysUserInfo, long> userRepository,
            IRepository<SysUserLoginAtt, long> userLoginAttRepository,
            //IUserClaimsPrincipalFactory<UserInfo> claimsPrincipalFactory,
            IUnitOfWorkManager unitOfWorkManager
            //IIocResolver iocResolver
            
            )
        {
            _userRepository = userRepository;
            _userLoginAttRepository = userLoginAttRepository;

            //_claimsPrincipalFactory = ;
            UnitOfWorkManager = unitOfWorkManager;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            //IocResolver = iocResolver;
        }

        #region
        /*

        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(login, tenancyName);
            await SaveLoginAttempt(result, tenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

  
        protected virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            TTenant tenant = null;
            if (!MultiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(LoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new AbpLoginResult<TTenant, TUser>(LoginResultType.TenantIsNotActive, tenant);
                }
            }

            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await UserManager.FindAsync(tenantId, login);
                if (user == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(LoginResultType.UnknownExternalLogin, tenant);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }
        */
        #endregion

        [UnitOfWork]
        public virtual async Task<LoginResult> LoginAsync(string userNameOrEmailAddress, string plainPassword, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, shouldLockout);
            await SaveLoginAttempt(result, userNameOrEmailAddress);
            return result;
        }

        protected virtual async Task<LoginResult> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            var user = await _userRepository.FirstOrDefaultAsync(q => q.UserAccout == userNameOrEmailAddress || q.Email== userNameOrEmailAddress);
            if (user == null)
            {
                return new LoginResult(LoginResultType.InvalidUserNameOrEmailAddress);
            }

            if (Common.Common.MD5String("utf-8", plainPassword).ToLower() != user.UserPassword)
            {
                return new LoginResult(LoginResultType.InvalidPassword);
            }



            if (user.UserStatus != 1)
            {
                return new LoginResult(LoginResultType.LockedOut, user);
            }
            return await CreateLoginResultAsync(user);

        }

        protected virtual async Task<LoginResult> CreateLoginResultAsync(SysUserInfo user)
        {
            #region
            /*
            if (!user.IsActive)
            {
                return new AbpLoginResult<TTenant, TUser>(LoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new AbpLoginResult<TTenant, TUser>(LoginResultType.UserEmailIsNotConfirmed);
            }

            if (await IsPhoneConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsPhoneNumberConfirmed)
            {
                return new AbpLoginResult<TTenant, TUser>(LoginResultType.UserPhoneNumberIsNotConfirmed);
            }
            */
            #endregion

            user.LastLoginTime = Clock.Now;
            await _userRepository.UpdateAsync(user);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserAccout));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserAccout));
            //identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));

            //var principle = new ClaimsPrincipal(identity);
            //var principal = new ClaimsPrincipal();
            //var principal = await new UserClaimsPrincipalFactory<UserInfo>(new UserManager<UserInfo>(), new IOptions<IdentityOptions>()).CreateAsync(user);
            //principal.Identities.First().AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString()));


            return new LoginResult(
                user,
                identity
            //principal.Identity as ClaimsIdentity
            );
        }

        protected virtual async Task SaveLoginAttempt(LoginResult loginResult, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var loginAttempt = new SysUserLoginAtt
                {
                    UserId = loginResult.User != null ? loginResult.User.Id : (long?)null,
                    UserNameOrEmailAddress = userNameOrEmailAddress,
                    Result = (Int16)loginResult.Result,
                    BrowserInfo = ClientInfoProvider.BrowserInfo,
                    ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                    ClientName = ClientInfoProvider.ComputerName,
                };

                await _userLoginAttRepository.InsertAsync(loginAttempt);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                await uow.CompleteAsync();
                //}
            }
        }
        #region
        /*
        protected virtual async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var user = await UserManager.FindByIdAsync(userId.ToString());

                    (await UserManager.AccessFailedAsync(user)).CheckErrors();

                    var isLockOut = await UserManager.IsLockedOutAsync(user);

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }


protected virtual async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
{
if (!UserManagementConfig.ExternalAuthenticationSources.Any())
{
    return false;
}

foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
{
    using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource<TTenant, TUser>>(sourceType))
    {
        if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
        {
            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await UserManager.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                if (user == null)
                {
                    user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                    user.TenantId = tenantId;
                    user.AuthenticationSource = source.Object.Name;
                    user.Password = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used
                    user.SetNormalizedNames();

                    if (user.Roles == null)
                    {
                        user.Roles = new List<UserRole>();
                        foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                        {
                            user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                        }
                    }

                    await UserManager.CreateAsync(user);
                }
                else
                {
                    await source.Object.UpdateUserAsync(user, tenant);

                    user.AuthenticationSource = source.Object.Name;

                    await UserManager.UpdateAsync(user);
                }

                await UnitOfWorkManager.Current.SaveChangesAsync();

                return true;
            }
        }
    }
}

return false;
}

protected virtual async Task<TTenant> GetDefaultTenantAsync()
{
var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == AbpTenant<TUser>.DefaultTenantName);
if (tenant == null)
{
    throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
}

return tenant;
}

protected virtual async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
{
if (tenantId.HasValue)
{
    return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
}

return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
}

protected virtual Task<bool> IsPhoneConfirmationRequiredForLoginAsync(int? tenantId)
{
return Task.FromResult(false);
}
*/
        #endregion
    }
}
