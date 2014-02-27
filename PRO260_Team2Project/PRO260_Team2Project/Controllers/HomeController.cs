using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using PRO260_Team2Project;

namespace PRO260_Team2Project.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<ImageOwner> imageList = null;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                if (ihc.ImageOwners.ToList().Count > 0)
                {
                    var images = ihc.ImageOwners.Take(15).ToList();
                    imageList = images;
                }
            }
            return View(imageList);
        }

        public ActionResult UploadImage()
        {
            return View();
        }

        public ActionResult MessageCenter()
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                List<Comment> Comments = ihc.Comments.Where(x => x.OwnerID == WebSecurity.CurrentUserId).Select(x => x).ToList();
                return View(Comments);
            }
        }
    }
}
