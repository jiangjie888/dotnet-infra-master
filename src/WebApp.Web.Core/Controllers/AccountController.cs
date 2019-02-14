using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApp.WebCore.Models.Account;

namespace WebApp.WebCore.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : WebAppControllerBase
    {
        //protected ICompositeViewEngine viewEngine;
        private readonly IRazorViewEngine _viewEngine;
        public AccountController(IRazorViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }


        //系统登录及注销
        [HttpGet]
        public IActionResult Login()
        {
            string clientState = HttpContext.Request.Query["state"].FirstOrDefault() ?? "";
            string clientId = HttpContext.Request.Query["clientId"].FirstOrDefault() ?? "";
            string returnUrl = HttpContext.Request.Query["returnUrl"].FirstOrDefault() ?? "";

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.PathBase.Value;
            }
            LoginClientModel model = new LoginClientModel()
            {
                ClientState = clientState,
                ClientId = clientId,
                ReturnUrl = returnUrl
            };
            return View("~/Views/Account/Login.cshtml",model);
        }

        //public async Task<IActionResult> Login()
        //{
        //    //var userAgent = Request.Headers["User-Agent"].FirstOrDefault();
        //    ////if (!string.IsNullOrEmpty(userAgent) && Regex.IsMatch(userAgent, @"MSIE [1-9]\."))
        //    ////{
        //    //var executor = HttpContext.RequestServices.GetRequiredService<ViewResultExecutor>();
        //    //var view = _viewEngine.GetView(null, "~/View.cshtml", true)?.View;
        //    //if (view != null)
        //    //{
        //    //    using (view as IDisposable)
        //    //    {
        //    //        await executor.ExecuteAsync(ControllerContext, view, ViewData, TempData, "text/html; charset=utf-8", 200);
        //    //    }

        //    //}
        //    ////}
        //    //return new EmptyResult();


        //    //viewName = viewName ?? ControllerContext.ActionDescriptor.ActionName;
        //    //ViewData.Model = model;

        //    //using (StringWriter sw = new StringWriter())
        //    //{
        //    //    IView view = viewEngine.FindView(ControllerContext, viewName, true).View;
        //    //    ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());

        //    //    view.RenderAsync(viewContext).Wait();

        //    //    return sw.GetStringBuilder().ToString();
        //    //}
        //}

        #region
        /*
        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ActionContext.ActionDescriptor.Name;

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                var engine = Resolver.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = engine.FindPartialView(ActionContext, viewName);

                ViewContext viewContext = new ViewContext(ActionContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());

                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();

                return sw.GetStringBuilder().ToString();
            }

        }

        public string Render(string viewPath)
        {
            return Render(viewPath, string.Empty);
        }

        public string Render<TModel>(string viewPath, TModel model)
        {
            var viewEngineResult = _viewEngine.GetView("~/", viewPath, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view {viewPath}");
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext();
                viewContext.HttpContext = _httpContextAccessor.HttpContext;
                viewContext.ViewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                { Model = model };
                viewContext.Writer = output;

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }

        public async Task<IActionResult> SignUp()
        {
            var userAgent = Request.Headers["User-Agent"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userAgent) && Regex.IsMatch(userAgent, @"MSIE [1-9]\."))
            {
                var services = HttpContext.RequestServices;
                var executor = services.GetRequiredService<ViewResultExecutor>();
                var viewEngine = services.GetRequiredService<IRazorViewEngine>();
                var view = viewEngine.GetView(null, "~/Pages/IeAlert.cshtml", true)?.View;
                if (view != null)
                {
                    using (view as IDisposable)
                    {
                        await executor.ExecuteAsync(ControllerContext, view, ViewData, TempData, "text/html; charset=utf-8", 200);
                    }

                }
            }
            return new EmptyResult();
            //...
        }
        */
        #endregion


        /// <summary>
        /// 系统注销
        /// </summary>
        [HttpPost]
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
