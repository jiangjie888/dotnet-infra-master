using Abp.AspNetCore.Mvc.Controllers;

namespace WebApp.WebCore.Controllers
{
    public abstract class WebAppControllerBase : AbpController
    {
        protected WebAppControllerBase()
        {
            LocalizationSourceName = WebAppConsts.LocalizationSourceName;
        }

        //protected void CheckErrors(IdentityResult identityResult)
        //{
        //    identityResult.CheckErrors(LocalizationManager);
        //}
    }
}
