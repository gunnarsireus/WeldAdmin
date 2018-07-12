using System;
using System.IO;
using System.Web.Mvc;
using SireusRR.Models;

namespace SireusRR.Controllers
{
    public class HandlerController : Controller
    {
        //
        // GET: /Images/

        public ActionResult Index(string arg1, string arg2)
        {
            PhotoSize size;
            switch (arg2.Replace("Size=", ""))
            {
                case "S":
                    size = PhotoSize.Small;
                    break;
                case "M":
                    size = PhotoSize.Medium;
                    break;
                case "L":
                    size = PhotoSize.Large;
                    break;
                default:
                    size = PhotoSize.Original;
                    break;
            }

            Session["PhotoID"] = arg1.Replace("PhotoID=", "");
            if (arg1 == "PhotoID=0")
            {
                var tmpPhotoId = PhotoManager.GetRandomPhotoId(PhotoManager.GetRandomAlbumId());
                arg1 = "PhotoID=" + tmpPhotoId;
                Session["PhotoID"] = tmpPhotoId.ToString();
                Session["RandomPhotoID"] = tmpPhotoId.ToString();
            }
            // Setup the PhotoID Parameter
            var id = 1;
            var stream = new MemoryStream();

            if (arg1.Substring(0, 7) == "PhotoID")
            {
                id = Convert.ToInt32(arg1.Replace("PhotoID=", ""));
                Session["PhotoID"] = id.ToString();
                PhotoManager.GetPhoto(id, size).CopyTo(stream);
            }
            else
            {
                id = Convert.ToInt32(arg1.Replace("AlbumID=", ""));
                PhotoManager.GetFirstPhoto(id, size).CopyTo(stream);
            }

            return File(stream.GetBuffer(), "image/png");
        }

        public ActionResult Download(string arg1, string arg2)
        {
            if ((Session["PhotoID"] != null))
            {
                ViewData["PhotoID"] = Session["PhotoID"].ToString();
            }
            else
            {
                Session["RandomPhotoID"] = PhotoManager.GetRandomPhotoId(PhotoManager.GetRandomAlbumId()).ToString();
                ViewData["PhotoID"] = Session["RandomPhotoID"].ToString();
            }
            ViewData["Size"] = "L";
            return View();
        }
    }
}