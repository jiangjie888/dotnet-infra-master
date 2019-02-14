using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application.SysUserInfos.Dto
{
	[AutoMapTo(typeof(SysUserInfo))]
    public class CreateUserInfoDto : IShouldNormalize
	{
		#region Model
		
		[StringLength(30)]
		public string UserName { set; get; }

		
		[StringLength(30)]
		public string UserAccout { set; get; }

		
		[StringLength(30)]
		public string UserPassword { set; get; }


        [StringLength(20)]
        public string Email { set; get; }

        [StringLength(200)]
        public string Remarks { set; get; }

        public Nullable<int> UserStatus { set; get; }

		#endregion

		#region Method
        public void Normalize()
        {

        }
        #endregion


        
	}
}