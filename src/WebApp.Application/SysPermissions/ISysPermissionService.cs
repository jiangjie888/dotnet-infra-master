using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.SysPermissions.Dto;
using WebApp.Core.SysPermissions;

namespace WebApp.Application.SysPermissions
{
    public interface ISysPermissionService : IApplicationService
    {
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        IQueryable<SysPermission> GetAllQuery();

        /// <summary>
        /// 获取权限所有数据
        /// </summary>
        /// <returns></returns>
        List<SysPermission> GetAllPermission();

        /// <summary>
        /// 权限核查处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void PermissionCheck(PermissionCheckDto input);

    }
}
