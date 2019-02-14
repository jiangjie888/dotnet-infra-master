using Abp.Modules;
using Abp.Reflection.Extensions;
using WebApp.Localization;

namespace WebApp
{
    public class WebAppCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
            Configuration.Auditing.IsEnabled = true;

            WebAppLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppCoreModule).GetAssembly());
        }
    }
}