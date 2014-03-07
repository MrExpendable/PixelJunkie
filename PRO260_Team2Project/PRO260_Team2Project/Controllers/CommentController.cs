using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PRO260_Team2Project.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        [HttpGet]
        public ActionResult AddComment(ImageOwner io)
        {
            Comment addition = new Comment();
            ViewBag.imgown = io;
            addition.OwnerID = io.OwnerID;
            addition.ImageID = io.OwnerID;
            addition.ImageOwner = io;
            return View(addition);
        }
        [HttpPost]
        public ActionResult AddComment(Comment com)
        {
            using (ImageHolderContext con = new ImageHolderContext())
            {
                com.PosterID = WebSecurity.CurrentUserId;
                com.TimeStamp = DateTime.Now;
                com.ImageOwner = con.ImageOwners.Where(x=> (x.ImageID==com.ImageID) && (x.OwnerID==com.OwnerID)).FirstOrDefault();
                con.Comments.Add(com);
                try
                {
                    con.SaveChanges();
                }
                catch (Exception e)
                {
                    var m = e.Message;
                }
            
                return RedirectToAction("DisplayImagePage", "Image", com.ImageOwner);
            }
        }

    }
}
