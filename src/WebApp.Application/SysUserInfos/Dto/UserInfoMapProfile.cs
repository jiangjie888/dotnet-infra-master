using AutoMapper;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application.SysUserInfos.Dto
{
    public class UserInfoMapProfile : Profile
	{
		public UserInfoMapProfile()
        {
            CreateMap<UserInfoDto, SysUserInfo>();

            CreateMap<CreateUserInfoDto, SysUserInfo>();
        }
	}
}