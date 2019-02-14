using Abp.Authorization;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebApp.Application.Common;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Abp.UI;
using WebApp.Application.SysPermissions;
using WebApp.Application.SysPermissions.Dto;
using WebApp.Configuration;
using Abp.Dependency;

namespace WebApp.Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MyAuthorizeFilterAttribute : ActionFilterAttribute
    {

        //private readonly string _syscode = System.Configuration.ConfigurationManager.AppSettings["_syscode"];
        //private readonly AppSettingsCfg _appsettings;
        //private readonly ISysPermissionService _permissionService;

        //public ILocalizationManager LocalizationManager { get; set; }


        

        //public MyAuthorizeFilterAttribute(ISysPermissionService permissionService,
        //                                  AppSettingsCfg appsettings)
        //{
        //    //LocalizationManager = NullLocalizationManager.Instance;
        //    _permissionService = permissionService;
        //    _appsettings = appsettings;
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            #region 优先排除不需要认证登录的属性
            MethodInfo methodinfo = (filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
            Type mtype = filterContext.ActionDescriptor.GetType();
            //匿名访问，直接返回
            if (ReflectionHelper.GetAttributesOfMemberAndType(methodinfo, mtype).OfType<IAbpAllowAnonymousAttribute>().Any()) return;
            if (ReflectionHelper.GetAttributesOfMemberAndType(methodinfo, mtype).OfType<IAllowAnonymous>().Any()) return;
            //ABP认证的忽略
            //var authorizeAttributes = ReflectionHelper.GetAttributesOfMemberAndType(methodinfo, mtype).OfType<IAbpAuthorizeAttribute>().ToArray();
            //if (!authorizeAttributes.Any())
            //{
            //    return;
            //}
            //var methodCustomAttributes = methodinfo.GetCustomAttributes(true).ToList(); //获得所有自定义的attributes标记
            #endregion

            var path = filterContext.HttpContext.Request.Path.ToString().ToLower();
            var isViewPage = false;//当前Action请求是否为具体的功能页


            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string token = filterContext.HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();

                //string cliamOrg = token.Split(".")[1];
                string re = JsonWebToken.Decode(token, "",false);

                //if ((filterContext.HttpContext.User.Claims.Count() > 0))
                //{

                //}
                throw new UserFriendlyException("认证失败","你的登录信息不存在或是过期,请重新登录");
                //throw new AbpAuthorizationException(LocalizationManager.GetString(WebAppConsts.LocalizationSourceName, "CurrentUserDidNotLoginToTheApplication"));
                //var resultJson = new JsonResult(new
                //{
                //    success = false,
                //    msg = "抱歉：你的登录信息不存在,请重新登录"
                //});
                //filterContext.Result = resultJson;
            }
            else
            {
                //根据验证判断进行处理
                this.AuthorizeCore(filterContext, isViewPage);
            }
        }
        ////权限判断业务逻辑
        protected virtual void AuthorizeCore(ActionExecutingContext filterContext, bool isViewPage)
        {

            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            //获取当前用户信息
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            var _appsettings = IocManager.Instance.Resolve<AppSettingsCfg>();
            var _permissionService = IocManager.Instance.Resolve<ISysPermissionService>();
            _permissionService.PermissionCheck(new PermissionCheckDto() { SystemCode = _appsettings.SystemCode, ControllerName = controllerName, ActionName = actionName });

            //return true;
        }

        #region 测试用
        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()) return;
        //}
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{

        //    //string cookieName = FormsAuthentication.FormsCookieName;

        //    //if (!filterContext.HttpContext.User.Identity.IsAuthenticated ||
        //    //    filterContext.HttpContext.Request.Cookies == null ||
        //    //    filterContext.HttpContext.Request.Cookies[cookieName] == null
        //    //)
        //    //{
        //    //    HandleUnauthorizedRequest(filterContext);
        //    //    return;
        //    //}

        //    //var authCookie = filterContext.HttpContext.Request.Cookies[cookieName];
        //    //var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //    //string[] roles = authTicket.UserData.Split(',');

        //    //var userIdentity = new GenericIdentity(authTicket.Name);
        //    //var userPrincipal = new GenericPrincipal(userIdentity, roles);

        //    //filterContext.HttpContext.User = userPrincipal;
        //    //base.OnAuthorization(actionContext);
        //}

        //protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        //{
        //    var httpContext = System.Web.HttpContext;
        //    //if (httpContext == null)
        //    //{
        //    //    base.HandleUnauthorizedRequest(actionContext);
        //    //    return;
        //    //}
        //    base.HandleUnauthorizedRequest(actionContext);


        //    httpContext.Response.StatusCode = httpContext.User.Identity.IsAuthenticated == false
        //                              ? (int)System.Net.HttpStatusCode.Unauthorized
        //                              : (int)System.Net.HttpStatusCode.Forbidden;

        //    //var content = new Result
        //    //{
        //    //     success = false,
        //    //     errs = new[] { "服务端拒绝访问：你没有权限，或者掉线了" }
        //    //};
        //    //httpContext.Response.Content = new StringContent(Json.Encode(content), Encoding.UTF8, "application/json");

        //    httpContext.Response.SuppressFormsAuthenticationRedirect = true;
        //    httpContext.Response.End();
        //}
        #endregion
    }
}
