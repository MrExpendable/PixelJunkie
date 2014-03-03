using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using PRO260_Team2Project.Models;

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
                    var images = ihc.ImageOwners.ToList();
                    imageList = images;
                }
            }
            return View(imageList);
        }

        public ActionResult SearchTags(string tag)
        {
            List<ImageOwner> imageList = null;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                var imgTags = ihc.ImageTags.Where(x => x.Tag == tag).ToList();
                foreach (ImageTag it in imgTags)
                {
                    imageList.Add(ihc.ImageOwners.Where(x => x.ImageID == it.ImageID).First());
                }
            }
            return View("Index", imageList);
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
