using System.Web.Mvc;
using SireusRR.Models;

namespace SireusRR.Controllers
{
    public class AlbumsController : Controller
    {
        //
        // GET: /Albums/
        public ActionResult Index()
        {
            return Json(PhotoManager.GetAlbums(), JsonRequestBehavior.AllowGet);
        }
    }
}