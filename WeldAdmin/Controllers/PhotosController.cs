using SireusRR.Models;
using System.Web.Mvc;

namespace SireusRR.Controllers
{
    public class PhotosController : Controller
    {
        //
        // GET: /Photos/

        public ActionResult Index(string id)  //id = albumId
        {
            return Json(PhotoManager.GetPhotos(int.Parse(id)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAlbumCaption(string id) // id = photoId
        {
            return Json(PhotoManager.GetAlbumCaption(int.Parse(id)), JsonRequestBehavior.AllowGet);
        }
    }
}