using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WebApp.Configuration;
using WebApp.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WebApp.WebCore.Configuration;
using WebApp.WebCore;

namespace WebApp.Web.Host.Startup
{
    [DependsOn(
        typeof(WebAppWebCoreModule))]
    public class WebAppWebModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public WebAppWebModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        //public override void PreInitialize()
        //{
        //    Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(WebAppConsts.ConnectionStringName);

        //    Configuration.Navigation.Providers.Add<WebAppNavigationProvider>();

        //    Configuration.Modules.AbpAspNetCore()
        //        .CreateControllersForAppServices(
        //            typeof(WebAppApplicationModule).GetAssembly()
        //        );
        //}

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppWebModule).GetAssembly());
        }
    }
}