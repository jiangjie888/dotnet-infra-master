using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WebApp.Web.Host.Startup;
namespace WebApp.Web.Host.Tests
{
    [DependsOn(
        typeof(WebAppWebModule),
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class WebAppWebTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppWebTestModule).GetAssembly());
        }
    }
}