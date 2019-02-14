using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application.Sessions.Dto
{
    [AutoMapFrom(typeof(SysUserInfo))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string UserName { set; get; }

        public string UserAccout { set; get; }

        public string Email { get; set; }
    }
}
