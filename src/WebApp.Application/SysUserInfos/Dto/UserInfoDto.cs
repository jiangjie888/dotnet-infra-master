using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application.SysUserInfos.Dto
{
	[AutoMapTo(typeof(SysUserInfo))]
    public class UserInfoDto : EntityDto<long>
	{
		#region Model
		/// <summary>
        /// 
        /// </summary>
		
		[StringLength(30)]
		public string UserName { set; get; }
		/// <summary>
        /// 
        /// </summary>
		
		[StringLength(30)]
		public string UserAccout { set; get; }
		/// <summary>
        /// 
        /// </summary>
		
		[StringLength(30)]
		public string UserPassword { set; get; }
        /// <summary>
        /// 
        /// </summary>

        [StringLength(20)]
        public string Email { set; get; }


        [StringLength(200)]
        public string Remarks { set; get; }

        public Nullable<int> UserStatus { set; get; }

        public DateTime? LastLoginTime { set; get; }

        #endregion

        #region Method
        public void Normalize()
        {

        }
        #endregion
		        
	}
}