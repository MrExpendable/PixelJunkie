using PRO260_Team2Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

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
            return View(addition);
        }
        [HttpPost]
        public ActionResult AddFlag(Flag f)
        {
            using (ImageHolderContext con = new ImageHolderContext())
            {
                f.FlaggerID = WebSecurity.CurrentUserId; 
                f.TimeOfFlag = DateTime.Now;
                int seekID = 0;
                int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out seekID);
                f.ImageID = seekID;
                f.Member = con.Members.Where(x => x.MemberID == WebSecurity.CurrentUserId).FirstOrDefault();
                f.Image = con.Images.Where(x => x.ImageID == f.ImageID).FirstOrDefault();
                con.Flags.Add(f);
                try
                {
                    con.SaveChanges();
                }
                catch (Exception e)
                {
                    var m = e.Message;
                }
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

        [Authorize(Roles="Admin")]
        public ActionResult veiwOverallImage()
        {
            int ImageID;
            int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out ImageID);
            Image img = null;
            using(ImageHolderContext ihc = new ImageHolderContext())
            {
                img = ihc.Images.Where(x => x.ImageID == ImageID).First();
                ViewBag.owners = ihc.ImageOwners.Include("Comments").Include("Auction_").Include("Likes1").Where(x => x.ImageID == ImageID).ToList();
            }
            return View(img);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult deleteCompleteImage()
        {
            ViewBag.submitted = false;
            int ImageID;
            int.TryParse((String)Url.RequestContext.RouteData.Values["id"], out ImageID);
            Image toDelete;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                toDelete = ihc.Images.Where(x => x.ImageID == ImageID).FirstOrDefault();
            }
            return View(toDelete);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult deleteCompleteImage(bool sure, string IDNum, int IDOrig)
        {
            int imageID;
            int.TryParse(IDNum, out imageID);
            if (imageID == IDOrig && sure)
            {
                using (ImageHolderContext ihc = new ImageHolderContext())
                {
                    if (ihc.Flags.Where(x => x.ImageID == IDOrig).Count() >= 1)
                    {
                        try
                        {
                            List<Auction_> AToDelete = new List<Auction_>();
                            AToDelete = ihc.Auction_.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Auction_.RemoveRange(AToDelete);
                            List<Comment> CToDelete = new List<Comment>();
                            CToDelete = ihc.Comments.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Comments.RemoveRange(CToDelete);
                            List<Flag> FToDelete = new List<Flag>();
                            FToDelete = ihc.Flags.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Flags.RemoveRange(FToDelete);
                            List<Purchase> PToDelete = new List<Purchase>();
                            PToDelete = ihc.Purchases.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Purchases.RemoveRange(PToDelete);
                            List<ImageOwner> IOToDelete = new List<ImageOwner>();
                            IOToDelete = ihc.ImageOwners.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Purchases.RemoveRange(PToDelete);
                           /* List<Image> IToDelete = new List<Image>();
                            IToDelete = ihc.Images.Where(x => x.ImageID == IDOrig).ToList();
                            ihc.Images.RemoveRange(IToDelete);*/
                            ihc.SaveChanges();
                            ViewBag.Message = "Success! Image " + IDOrig + " has been deleted!";
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = "Something went wrong with the deletion: " + e.Message;
                        }
                    }else
                    {
                        ViewBag.Message ="This image isn't flagged, why are you purging it completely?";
                    }
                }
            }else
            {
                        ViewBag.Message ="You missed the confirmation, such drastic action will not be acted upon without the proper convermation.";
            }
            ViewBag.submitted = true;
            return View();
        }

    }
}
