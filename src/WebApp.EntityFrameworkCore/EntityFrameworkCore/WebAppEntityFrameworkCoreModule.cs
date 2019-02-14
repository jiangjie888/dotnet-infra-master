using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace WebApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(WebAppCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class WebAppEntityFrameworkCoreModule : AbpModule
    {

        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<WebAppDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppEntityFrameworkCoreModule).GetAssembly());
        }
    }
}