using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Core.SysApis;

namespace WebApp.Application.SysApis.Dto
{
    public class ApiMapProfile : Profile
    {
        public ApiMapProfile()
        {
            CreateMap<ApiDto, SysApi>();

            CreateMap<CreateApiDto, SysApi>();
        }
    }
}
