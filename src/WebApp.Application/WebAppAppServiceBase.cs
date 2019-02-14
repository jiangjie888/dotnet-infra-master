using Abp.Application.Services;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using System;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.SysUserInfos;

namespace WebApp
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class WebAppAppServiceBase : ApplicationService
    {
        //private IRepository<SysUserInfo, long> _userbaseRepository { get; set; }

        //隐藏父类的AbpSession
        public new IAbpSessionExtension AbpSession { get; set; }

        protected WebAppAppServiceBase()
        {
            LocalizationSourceName = WebAppConsts.LocalizationSourceName;
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
        }
    }
}