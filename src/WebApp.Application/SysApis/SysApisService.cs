using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Application.SysApis.Dto;
using WebApp.Core.SysApis;

namespace WebApp.Application.SysApis
{
    public class SysApisService : WebAppCrudAppService<SysApi, ApiDto, Guid, PagedAndSortedResultRequestDto, CreateApiDto, ApiDto, EntityDto<Guid>, ApiDto>, ISysApisService
    {
        private readonly ICacheManager _cacheManager;

        public SysApisService(IRepository<SysApi, Guid> repository, ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 从数据库查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysApi> GetAllListFromDatabase()
        {
            IEnumerable<SysApi> data = Repository.GetAllList();
            return data;
        }

        #region 查询
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        public IQueryable<SysApi> GetAllQuery()
        {
            return Repository.GetAll();
        }

        /// <summary>
        /// 获取权限所有数据
        /// </summary>
        /// <returns></returns>
        public List<SysApi> GetAllApi()
        {
            IEnumerable<SysApi> query = _cacheManager.GetCache("ApisCache").Get("All", () => GetAllListFromDatabase()) as IEnumerable<SysApi>;
            var outputdata = query.ToList();
            return outputdata;
        }
        #endregion
    }
}
