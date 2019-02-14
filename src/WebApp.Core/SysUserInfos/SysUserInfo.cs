using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.SysUserInfos
{
    [Table("Sys_UserInfo")]
    public class SysUserInfo : FullAuditedEntity<long>
    {

        #region Model
        /// <summary>
        /// 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string UserAccout { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string UserPassword { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> UserStatus { set; get; }

        public string Email { set; get; }


        public string Remarks { set; get; }


        public DateTime? LastLoginTime { set; get; }


        #endregion


    }
}