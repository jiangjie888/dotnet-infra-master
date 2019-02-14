using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.SysUserInfos.Dto;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application.SysUserInfos
{
    public interface ISysUserInfosService : IAsyncCrudAppService<UserInfoDto, long, PagedAndSortedResultRequestDto, CreateUserInfoDto, UserInfoDto, EntityDto<long>, UserInfoDto>
    {

        Task<SysUserInfo> FindByNameOrEmailAsync(string userNameOrEmailAddress);

        bool IsLockedOut(SysUserInfo u);
    }
}
