﻿using PRO260_Team2Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PRO260_Team2Project.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        #region ImageManagement
        //[Authorize]
        [HttpPost]
        public ActionResult StoreImage(HttpPostedFileBase file, int? price, string title, string caption)
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
                            DateTime time = DateTime.Now;
                            //this is using the test user created in the Index action in home
                            int ID = 11;
                            //int ID = WebSecurity.CurrentUserId;
                            Image newImage = new Image();
                            newImage.Image1 = imageArray;
                            newImage.DateOfUpload = time;
                            newImage.OriginalPosterID = ID;
                            ihc.Images.Add(newImage);
                            ihc.SaveChanges();

                            ImageOwner imgOwn = new ImageOwner();
                            if (price != null)
                            {
                                imgOwn.Price = price;
                            }
                            if (title != null)
                            {
                                imgOwn.Title = title;
                            }
                            if (caption != null)
                            {
                                imgOwn.Caption = caption;
                            }
                            Image img = ihc.Images.Where(x => x.OriginalPosterID == ID && x.DateOfUpload == time).First();
                            imgOwn.Image = img;
                            imgOwn.ImageID = img.ImageID;
                            imgOwn.Member = ihc.Members.Where(x => x.MemberID == ID).First();
                            imgOwn.OwnerID = ID;
                            imgOwn.TimeStamp = time;
                            ihc.ImageOwners.Add(imgOwn);
                            ihc.SaveChanges();
                        }
                        ViewBag.message = "Image successfully added";
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
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

        [Authorize]
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
        #endregion

        #region TagManagement
        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult DeleteTag(ImageOwner imgOwn, string tag)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                ImageTag foundTag = ihc.ImageTags.Where(x => x.ImageID == imgOwn.ImageID && x.Tag.Equals(tag)).FirstOrDefault();
                if (foundTag != null)
                    ihc.ImageTags.Remove(foundTag);
                ViewBag.message = "Tag successfully deleted";
                ihc.SaveChanges();
            }
            return View("DisplayImagePage", imgOwn);
        }
        #endregion

        public ActionResult DisplayImagePage(ImageOwner imgOwn)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
               // byte[] img = ihc.Images.Where(x => x.ImageID == imgOwn.ImageID).FirstOrDefault().Image1;
                var base64 = Convert.ToBase64String(ihc.Images.Where(x => x.ImageID == imgOwn.ImageID).FirstOrDefault().Image1);
                var img = String.Format("data:image/gif;base64,{0}", base64);
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
