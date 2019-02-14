using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace WebApp.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<WebAppDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<WebAppDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }

        //public static void Configure(
        //    DbContextOptionsBuilder<WebAppDbContext> dbContextOptions, 
        //    string connectionString
        //    )
        //{
        //    /* This is the single point to configure DbContextOptions for WebAppDbContext */
        //    dbContextOptions.UseMySql(connectionString);
        //}
    }
}
