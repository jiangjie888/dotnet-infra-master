using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Core.AuditLogs;

namespace WebApp.EntityFrameworkCore
{
    public class WebAppDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...

        public virtual DbSet<WebApp.Core.SysUserInfos.SysUserLoginAtt> SysUserLoginAtt { get; set; }

        public virtual DbSet<WebApp.Core.SysUserInfos.SysUserInfo> SysUserInfo { get; set; }

        public virtual DbSet<WebApp.Core.SysApis.SysApi> SysApi { get; set; }

        public virtual DbSet<WebApp.Core.SysApplications.SysApplication> SysApplication { get; set; }
        

        public virtual DbSet<WebApp.Core.SysPermissions.SysPermission> SysPermission { get; set; }


        #region
        /// <summary>
        /// Audit logs.
        /// </summary>
        public virtual DbSet<WebApp.Core.AuditLogs.AuditLog> AuditLogs { get; set; }
        #endregion

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region AuditLog.Set_MaxLengths

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ServiceName)
                .HasMaxLength(AuditLog.MaxServiceNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.MethodName)
                .HasMaxLength(AuditLog.MaxMethodNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Parameters)
                .HasMaxLength(AuditLog.MaxParametersLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientIpAddress)
                .HasMaxLength(AuditLog.MaxClientIpAddressLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientName)
                .HasMaxLength(AuditLog.MaxClientNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.BrowserInfo)
                .HasMaxLength(AuditLog.MaxBrowserInfoLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Exception)
                .HasMaxLength(AuditLog.MaxExceptionLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.CustomData)
                .HasMaxLength(AuditLog.MaxCustomDataLength);

            #endregion
        }
    }
}
