using System.Web.Mvc;

namespace Dhobi.Admin.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Dhobi | Home";
            return View("Login");
        }
    }
}