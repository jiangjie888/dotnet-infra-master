using Microsoft.AspNetCore.Mvc;
using WebApp.WebCore.Controllers;

namespace WebApp.Web.Host.Controllers
{
    public class HomeController : WebAppControllerBase
    {
        public IActionResult Index()
        {
            return Redirect(Request.PathBase.Value + "/swagger");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}