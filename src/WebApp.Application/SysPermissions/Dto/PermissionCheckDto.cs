using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.SysPermissions.Dto
{
    public class PermissionCheckDto
    {

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

    }
}
