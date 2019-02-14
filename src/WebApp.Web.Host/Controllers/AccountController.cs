using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.WebCore.Controllers;

namespace WebApp.Web.Host.Controllers
{
    //[Route("api/[controller]/[action]")]
    public class AccountController : WebAppControllerBase
    {

        #region 系统登录及注销

        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.PathBase.Value;
            }

            //return RedirectToAction("Login");
            return View("~/Views/Account/Login.cshtml");
            //return View(
            //    new LoginFormViewModel
            //    {
            //        ReturnUrl = returnUrl
            //    });
        }


        /// <summary>
        /// 系统注销
        /// </summary>
        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        #endregion



    }
}
