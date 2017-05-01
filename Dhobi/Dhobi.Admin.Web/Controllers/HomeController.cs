using System.Web.Mvc;

namespace Dhobi.Admin.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route()]
        public ActionResult Index()
        {
            return RedirectPermanent("/Login");
        }

        [Route("~/Login")]
        public ActionResult Login()
        {
            ViewBag.Title = "Dobi | Login";
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