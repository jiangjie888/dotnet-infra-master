using System;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApp.Configuration;
using WebApp.EntityFrameworkCore;
using WebApp.WebCore.Authentication.JwtBearer;
using WebApp.WebCore.Configuration;


#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#elif FEATURE_SIGNALR_ASPNETCORE
using Abp.AspNetCore.SignalR;
#endif

namespace WebApp.WebCore
{
    [DependsOn(
         typeof(WebAppApplicationModule),
         typeof(WebAppEntityFrameworkCoreModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#elif FEATURE_SIGNALR_ASPNETCORE
        ,typeof(AbpAspNetCoreSignalRModule)
#endif
     )]
    public class WebAppWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WebAppWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                WebAppConsts.ConnectionStringName
            );

            // Use database for language management
            //Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(WebAppApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
            ConfigureAppSettings();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            //tokenAuthConfig.Expiration = TimeSpan.FromMinutes(60);
            tokenAuthConfig.Expiration = TimeSpan.FromSeconds(30);
        }

        private void ConfigureAppSettings()
        {
            IocManager.Register<AppSettingsCfg>();
            var appSettingsConfig = IocManager.Resolve<AppSettingsCfg>();

            appSettingsConfig.SystemCode = _appConfiguration["AppSettings:SystemCode"];
            appSettingsConfig.FtpIP = _appConfiguration["AppSettings:FtpIP"];
            appSettingsConfig.FtpUid = _appConfiguration["AppSettings:FtpUid"];
            appSettingsConfig.FtpPwd = _appConfiguration["AppSettings:FtpPwd"];
            appSettingsConfig.FtpVUid = _appConfiguration["AppSettings:FtpVUid"];
            appSettingsConfig.FtpVPwd = _appConfiguration["AppSettings:FtpVPwd"];
            appSettingsConfig.FtpDir = _appConfiguration["AppSettings:FtpDir"];
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppWebCoreModule).GetAssembly());
        }
    }
}
