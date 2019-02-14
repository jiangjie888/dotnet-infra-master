using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Application.SysApis.Dto;
using WebApp.Core.SysApis;

namespace WebApp.Application.SysApis
{
    public interface ISysApisService : IAsyncCrudAppService<ApiDto, Guid, PagedAndSortedResultRequestDto, CreateApiDto, ApiDto, EntityDto<Guid>, ApiDto>
    {
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        IQueryable<SysApi> GetAllQuery();

        /// <summary>
        /// 获取权限所有数据
        /// </summary>
        /// <returns></returns>
        List<SysApi> GetAllApi();
    }
}
