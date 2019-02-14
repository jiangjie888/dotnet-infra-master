using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.SysApplications
{
    [Table("Sys_Application")]
    public class SysApplication : FullAuditedEntity<Guid>
    {
        #region Model

        /// <summary>
        /// 本地系统/第三方系统
        /// </summary>
        public string Type { set; get; }


        /// <summary>
        /// 接入应用编码
        /// </summary>
        public string ApplicationCode { set; get; }


        /// <summary>
        /// 接入应用名称
        /// </summary>
        public string ApplicationName { set; get; }

        /// <summary>
        /// 首页地址
        /// </summary>
        public string MainUrl { set; get; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string RedirectUrl { set; get; }



        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }


        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { set; get; } = true;

        #endregion
    }
}
