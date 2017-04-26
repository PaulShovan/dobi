using System.Web.Mvc;

namespace Dhobi.Admin.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Dobi | Home";
            return View("Login");
        }
        public ActionResult Admin()
        {
            ViewBag.Title = "Dobi | Admin";
            return View("Admin");
        }
    }
}