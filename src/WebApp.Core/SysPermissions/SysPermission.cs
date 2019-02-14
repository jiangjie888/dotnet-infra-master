using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.SysPermissions
{
    [Table("Sys_Permission")]
    public class SysPermission:Entity<long>
    {
        #region Model

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { set; get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { set; get; }


        /// <summary>
        /// 系统API的ID
        /// </summary>
        public Nullable<Guid> ApiId { set; get; }


        #endregion


    }
}