using System.Web.Mvc;

namespace WeldAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}