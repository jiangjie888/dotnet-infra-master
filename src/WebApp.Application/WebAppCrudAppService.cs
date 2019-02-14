using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Stbis.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WebApp.Application.Authorization;
using WebApp.Core.Extensions;
using WebApp.Core.SysUserInfos;

namespace WebApp.Application
{
    [MyAuthorizeFilter]
    public abstract class WebAppCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {

        //隐藏父类的AbpSession
        public new IAbpSessionExtension AbpSession { get; set; }

    
        //private IRepository<SysUserInfo, long> _userbaseRepository { get; set; }

        protected WebAppCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
            LocalizationSourceName = WebAppConsts.LocalizationSourceName;
            //_userbaseRepository = userbaseRepository;

        }

        protected virtual Task<SysUserInfo> GetCurrentUserAsync()
        {
            //Guid userid = new Guid(AbpSession.MyUserId);
            //IocManager.Instance.Register<IRepository<SysUserInfo, long>>();
            var _userbaseRepository = IocManager.Instance.Resolve<IRepository<SysUserInfo, long>>();

            var user = _userbaseRepository.FirstOrDefaultAsync(u => u.Id == AbpSession.GetUserId());
            if (user == null)
            {
                throw new Exception("当前用户不存在!");
            }
            return user;

            //Guid userid = new Guid(AbpSession.MyUserId);
            //var user = _userbaseRepository.FirstOrDefaultAsync(u => u.Id == userid);
            //if (user == null)
            //{
            //    throw new Exception("当前用户不存在!");
            //}
            //return user;
        }

        //public override async Task<TEntityDto> Create(TCreateInput input)
        //{
        //    CheckCreatePermission();

        //    var entity = MapToEntity(input);

        //    Repository.Insert(entity);
        //    CurrentUnitOfWork.SaveChanges();

        //    return MapToEntityDto(entity);
        //}

        public void CreateByBatch(List<TCreateInput> inputs)
        {
            CheckCreatePermission();

            foreach (var input in inputs)
            {
                var entity = MapToEntity(input);

                Repository.Insert(entity);
            }

            CurrentUnitOfWork.SaveChanges();

        }

        public async void UpdateByBatch(List<TUpdateInput> inputs)
        {
            CheckUpdatePermission();

            foreach (var input in inputs)
            {
                var entity = Repository.Get(input.Id);

                MapToEntity(input, entity);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }


        public async void UpdateDeleteByBatch(List<TDeleteInput> inputs)
        {
            CheckDeletePermission();


            await Repository.DeleteAsync(t => inputs.Select(i => i.Id).Contains(t.Id));

        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetAllByQuery(TGetAllInput input, string filterJson, string include)
        {
            CheckGetAllPermission();

            Expression<Func<TEntity, bool>> exp = getWhere(filterJson);

            var query = CreateIncludeQuery(input, include).Where(exp);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            try
            {
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var dtolist = entities.Select(MapToEntityDto).ToList();
                return new PagedResultDto<TEntityDto>(
                    totalCount,
                    dtolist
                );
            }
            catch (Exception exp1)
            {
                throw new Exception(exp1.Message);
            }



        }

        /// <summary>
        /// 解析前端传过来的查询条件
        /// </summary>
        /// <param name="filterJson"></param>
        /// <returns></returns>
        private Expression<Func<TEntity, bool>> getWhere(string filterJson)
        {
            FilterGroup filter = new FilterGroup();

            if (!string.IsNullOrEmpty(filterJson))
            {
                var ser = new DataContractJsonSerializer(typeof(FilterGroup));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(filterJson));
                filter = (FilterGroup)ser.ReadObject(ms);
            }

            QueryableSearcher<TEntity> queryType = new QueryableSearcher<TEntity>(filter);
            Expression<Func<TEntity, bool>> exp = t => 1 == 1;

            try
            {
                exp = queryType.Search();
            }
            catch (NullReferenceException nullExp)
            {
                throw nullExp;
            }

            return exp;
        }

        private IQueryable<TEntity> CreateIncludeQuery(TGetAllInput input, string include)
        {
            IQueryable<TEntity> query;
            if (!string.IsNullOrEmpty(include))
            {
                query = CreateFilteredQuery(input).Include(include);
            }
            else
            {
                query = CreateFilteredQuery(input);
            }

            return query;
        }
    }
}