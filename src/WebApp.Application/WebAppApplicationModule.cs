using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;

namespace WebApp
{
    [DependsOn(
        typeof(WebAppCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class WebAppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //    //base.PreInitialize();
            //    IocManager.Register<ICacheManager, AbpRedisCacheManager>();
            //    //如果Redis在本机,并且使用的默认端口,下面的代码可以不要
            //    //Configuration.Modules. = "Abp.Redis.Cache";

            //配置使用Redis缓存
            //Configuration.Modules.AbpConfiguration.DefaultNameOrConnectionString = "Abp.Redis.Cache";
            //Configuration.Caching.UseRedis();

            //配置所有Cache的默认过期时间30天
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(30);
            });

            //配置指定的Cache过期时间为10分钟
            Configuration.Caching.Configure("LoginTokenCache", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromSeconds(45);
            });

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebAppApplicationModule).GetAssembly());
        }
    }
}