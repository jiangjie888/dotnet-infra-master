using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Core.SysApis;

namespace WebApp.Application.SysApis.Dto
{
    [AutoMapTo(typeof(SysApi))]
    public class ApiDto : EntityDto<Guid>
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
