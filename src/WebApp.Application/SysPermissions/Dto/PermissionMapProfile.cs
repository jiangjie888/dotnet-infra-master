using AutoMapper;
using WebApp.Core.SysPermissions;

namespace WebApp.Application.SysPermissions.Dto
{
    public class PermissionsMapProfile : Profile
	{
		public PermissionsMapProfile()
        {
            CreateMap<PermissionDto, SysPermission>();

            CreateMap<CreatePermissionDto, SysPermission>();
        }
	}
}