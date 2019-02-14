using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using WebApp.Core.SysPermissions;

namespace WebApp.Application.SysPermissions.Dto
{
	[AutoMapTo(typeof(SysPermission))]
    public class CreatePermissionDto : IShouldNormalize
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

        #region Method
        public void Normalize()
        {

        }
        #endregion


        
	}
}