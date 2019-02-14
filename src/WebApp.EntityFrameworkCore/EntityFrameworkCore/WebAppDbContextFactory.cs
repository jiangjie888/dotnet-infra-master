using WebApp.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WebApp.Core.Web;

namespace WebApp.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class WebAppDbContextFactory : IDesignTimeDbContextFactory<WebAppDbContext>
    {
        public WebAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WebAppDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(WebAppConsts.ConnectionStringName)
            );

            return new WebAppDbContext(builder.Options);
        }
    }
}