using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.SysApis
{
    [Table("Sys_Api")]
    public class SysApi : FullAuditedEntity<Guid>
    {
        #region Model
        /// <summary>
        /// 所属项目的ID
        /// </summary>
        public string SystemCode { set; get; }


        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string ActionName { set; get; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }


        /// <summary>
        /// 该Action是否返回页面
        /// </summary>
        public Nullable<bool> IsViewPage { set; get; }

        #endregion
    }
}
