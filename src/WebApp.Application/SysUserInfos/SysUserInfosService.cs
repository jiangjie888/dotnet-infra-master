using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.SysUserInfos.Dto;
using WebApp.Core.SysUserInfos;
using WebApp.Application.Authorization;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Application.SysUserInfos
{
    //[AbpAuthorize]
    
    public class SysUserInfosService : WebAppCrudAppService<SysUserInfo, UserInfoDto, long, PagedAndSortedResultRequestDto, CreateUserInfoDto, UserInfoDto, EntityDto<long>, UserInfoDto>, ISysUserInfosService
    {
        
        public SysUserInfosService(IRepository<SysUserInfo, long> repository) : base(repository)
        {

        }
  
        public async Task<SysUserInfo> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {

            string t1 = AbpSession.MyUserId;
            SysUserInfo user = await Repository.FirstOrDefaultAsync(u => u.UserAccout == userNameOrEmailAddress || u.Email == userNameOrEmailAddress);
            user.Remarks = "172.16.1.3";
            //user.Remarks = "172.16.4.121";
            
            return user;
        }

        [DisableAuditing]
        public bool IsLockedOut(SysUserInfo u)
        {
            return (u.UserStatus == 1) ? true : false;
        }


        [AbpAllowAnonymous]
        public override Task<UserInfoDto> Create(CreateUserInfoDto input)
        {
            input.UserPassword = WebApp.Common.Common.MD5String("utf-8", input.UserPassword).ToLower();
            return base.Create(input);
        }


    }
}
