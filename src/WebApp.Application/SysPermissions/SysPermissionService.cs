using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.SysApis;
using WebApp.Application.SysPermissions.Dto;
using WebApp.Core.SysApis;
using WebApp.Core.SysPermissions;

namespace WebApp.Application.SysPermissions
{
    public class SysPermissionService : WebAppAppServiceBase,ISysPermissionService
    //, IEventHandler<EntityChangedEventData<SysPermission>>, ITransientDependency
    {
        private readonly IRepository<SysPermission, long> _permissionRepository;
        //private readonly IRepository<SysApi, Guid> _apiRepository;
        private readonly ISysApisService _apiRepository;
        private readonly ICacheManager _cacheManager;
        
        public SysPermissionService(
            IRepository<SysPermission, long> permissionRepository,
            ISysApisService apiRepository,
            ICacheManager cacheManager)

        {
            _permissionRepository = permissionRepository;
            _apiRepository = apiRepository;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 从数据库查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SysPermission> GetAllListFromDatabase()
        {
            IEnumerable<SysPermission> data = _permissionRepository.GetAllList();
            return data;
        }

        #region 查询
        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        public IQueryable<SysPermission> GetAllQuery()
        {
            return _permissionRepository.GetAll();
        }

        /// <summary>
        /// 获取权限所有数据
        /// </summary>
        /// <returns></returns>
        public List<SysPermission> GetAllPermission()
        {
            IEnumerable<SysPermission> query = _cacheManager.GetCache("PermissionsCache").Get("All", () => GetAllListFromDatabase()) as IEnumerable<SysPermission>;
            //var outputdata = query.Where(q => q.IsActive == true).OrderBy(d => d.Sort).ToList().MapTo<List<Dict>>();
            //List<DictDto> data = outputdata.MapTo<List<DictDto>>();
            var outputdata = query.ToList();
            return outputdata;
        }
        #endregion

        public void PermissionCheck(PermissionCheckDto input)
        {
            int count = _apiRepository.GetAllApi().Count(q => q.SystemCode == input.SystemCode && q.ControllerName == input.ControllerName && q.ActionName == input.ActionName);
            //API不存在受控的列表中，直接返回;如果是受按的API，则进行鉴权
            if (count > 0)
            {
                //用户API鉴权
                Guid apiid = _apiRepository.GetAllApi().First(q => q.SystemCode == input.SystemCode && q.ControllerName == input.ControllerName && q.ActionName == input.ActionName).Id;
                long? userid = AbpSession.UserId;

                //ArrayList gridids = GetNextGridId(userid, level);
                //string[] arrGridids = (string[])gridids.ToArray(typeof(string));

                if (GetAllPermission().Count(q => q.ApiId == apiid && q.UserId == userid) == 0)
                {
                    throw new UserFriendlyException("你没有足够的权限操作资源");
                }
                //用户的角色API鉴权
            }

        }



    }
}
