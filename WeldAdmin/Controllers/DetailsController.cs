using System;
using System.Web.Mvc;
using SireusRR.Models;
using System.Collections.Generic;

namespace SireusRR.Controllers
{
    public class DetailsController : Controller
    {
        //
        // GET: /Details/

        public ActionResult GetPhotos(string id)
        {
            if (id=="0")
            {
                var list = new List<Photo>();
                int idd = 0;
                
                if (Session["RandomPhotoID"]!=null && int.TryParse(Session["RandomPhotoID"].ToString(), out idd))
                {
                    list.Add(PhotoManager.GetPhoto(idd));
                }
                else
                {
                    var tmpPhotoId = PhotoManager.GetRandomPhotoId(PhotoManager.GetRandomAlbumId());
                    list.Add(PhotoManager.GetPhoto(tmpPhotoId));
                }
                return Json(list, JsonRequestBehavior.AllowGet);                
            }
            return Json(PhotoManager.GetPhotos(int.Parse(id)), JsonRequestBehavior.AllowGet);
        }
    }
}