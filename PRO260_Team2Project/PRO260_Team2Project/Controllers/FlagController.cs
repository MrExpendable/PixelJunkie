using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRO260_Team2Project;

namespace PRO260_Team2Project.Controllers
{
    public class FlagController : Controller
    {
        //
        // GET: /Flag/
        [HttpGet]
        public ActionResult AddFlag()
        {
            Flag addition = new Flag();
         //   int seekID = 0;
          //  int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out seekID);
         //   addition.ImageID = seekID;
            return View(addition);
        }
        [HttpPost]
        public ActionResult AddFlag(Flag f)
        {
            using (ImageHolderContext con = new ImageHolderContext())
            {
                f.FlaggerID = 1; //1 used for testing purposes, will take ID of user logged in for the session later
                f.TimeOfFlag = DateTime.Now;
                int seekID = 0;
                int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out seekID);
                f.ImageID = seekID;
                con.Flags.Add(f);
                con.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult removeFlagsOnImage()
        {
            //Should only be available for admin from the image page. When user pages and messages are up there will be a way to delete specific flags
            int ImageID;
            int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out ImageID);
            using (ImageHolderContext con = new ImageHolderContext())
            {
                var imageFlags =
                    from F in con.Flags
                    where F.ImageID == ImageID
                    select F;
                foreach (Flag F in imageFlags)
                {
                    con.Flags.Remove(F);
                }
                con.SaveChanges();
            }
            return RedirectToAction("Index", "Home");//Right now this redirects to home, it will redirect back to the image page.
        }

    }
}
