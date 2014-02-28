using PRO260_Team2Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRO260_Team2Project;

namespace PRO260_Team2Project.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        [HttpPost]
        public ActionResult StoreImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    file.InputStream.CopyTo(stream);
                    byte[] imageArray = stream.GetBuffer();
                    if (ModelState.IsValid)
                    {
                        using (ImageHolderContext ihc = new ImageHolderContext())
                        {
                            Image newImage = new Image();
                            newImage.Image1 = imageArray;
                            newImage.DateOfUpload = DateTime.Now;
                            //we're entering 1 for testing purposes, we would normally use the current user's imageID
                            newImage.OriginalPosterID = 1;
                            ihc.Images.Add(newImage);
                            ihc.SaveChanges();
                        }
                        ViewBag.message = "Image successfully added";
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DeleteImageID(int imageID)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                var toDelete = ihc.Images.Where(x => x.ImageID == imageID).FirstOrDefault();
                if (toDelete != null)
                {
                    ihc.Images.Remove(toDelete);
                    ViewBag.message = "Image successfully deleted";
                }
                ihc.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DeleteImage(Image img)
        {
            if (img != null)
            {
                using (ImageHolderContext ihc = new ImageHolderContext())
                {
                    var toDelete = ihc.Images.Where(x => x.ImageID == img.ImageID).FirstOrDefault();
                    if (toDelete != null)
                    {
                        ihc.Images.Remove(toDelete);
                        ViewBag.message = "Image successfully deleted";
                    }
                    ihc.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddTag(ImageOwner imgOwn, string tag)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                ImageTag newTag = new ImageTag();
                newTag.ImageID = imgOwn.ImageID;
                newTag.Tag = tag;
                ihc.ImageTags.Add(newTag);
                ViewBag.message = "Tag successfully added";
                ihc.SaveChanges();
            }
            return View("DisplayImagePage", imgOwn);
        }

        public ActionResult DisplayImagePage(ImageOwner imgOwn)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                byte[] img = ihc.Images.Where(x => x.ImageID == imgOwn.ImageID).FirstOrDefault().Image1;
                ViewBag.Image = img;
                List<string> tagList = ihc.ImageTags.Where(x => x.ImageID == imgOwn.ImageID).Select(x => x.Tag).ToList();
                ViewBag.Tags = tagList;
                List<Comment> Comments = ihc.Comments.Where(x => x.ImageID == imgOwn.ImageID && x.OwnerID == imgOwn.OwnerID).Select(x => x).ToList();
                ViewBag.Comments = Comments;
            }
            return View(imgOwn);
        }

        public ActionResult LikeImage(ImageOwner imgOwn)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                ImageOwner image = ihc.ImageOwners.Where(x => x.ImageID == imgOwn.ImageID && x.OwnerID == imgOwn.OwnerID).FirstOrDefault();
                if (image != null)
                {
                    image.Likes = image.Likes + 1;
                    ihc.SaveChanges();
                }
            }
            return View("DisplayImagePage", imgOwn);
        }
    }
}
