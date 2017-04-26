using System.Web.Mvc;

namespace Dhobi.Admin.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("~/Login")]
        public ActionResult Index()
        {
            ViewBag.Title = "Dobi | Home";
            return View("Login");
        }

        [Route("~/Admin")]
        public ActionResult Admin()
        {
            ViewBag.Title = "Dobi | Admin";
            return View();
        }
    }
}