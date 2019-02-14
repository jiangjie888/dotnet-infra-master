using System;
using System.Threading.Tasks;
using Abp.TestBase;
using WebApp.EntityFrameworkCore;
using WebApp.Tests.TestDatas;

namespace WebApp.Tests
{
    public class WebAppTestBase : AbpIntegratedTestBase<WebAppTestModule>
    {
        public WebAppTestBase()
        {
            UsingDbContext(context => new TestDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<WebAppDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<WebAppDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<WebAppDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<WebAppDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<WebAppDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<WebAppDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<WebAppDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<WebAppDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
