using PRO260_Team2Project;
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
        public ActionResult StoreImage(HttpPostedFileBase file, long price, string title, string caption, long startingBid, int daysOfAuction, String listSelection)
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
                            //int ID = 11;
                            int ID = WebSecurity.CurrentUserId;
                            Image newImage = new Image();
                            newImage.Image1 = imageArray;
                            newImage.DateOfUpload = time;
                            newImage.OriginalPosterID = ID;
                            ihc.Images.Add(newImage);
                            ihc.SaveChanges();

                            ImageOwner imgOwn = new ImageOwner();

                                imgOwn.Price = price;

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
                            if (price > 0)
                            {
                                imgOwn.isForSale = true;
                            }
                            else
                            {
                                imgOwn.isForSale = false;
                            }

                            if (listSelection.Equals("BidandBuy"))
                            {
                                imgOwn.isAuction = true;
                                AddAuction(startingBid, daysOfAuction, imgOwn);
                            }
                            else
                            {
                                imgOwn.isAuction = false;
                            }


                            ihc.ImageOwners.Add(imgOwn);
                            ihc.SaveChanges();

                            
                        }
                        ViewBag.message = "Image successfully added";
                    }
                }
            }
            return RedirectToAction("Profile", "Account");
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

        #region Search
        public ActionResult Search(String searchString = "", int page = 1, int resultsPerPage = 25, int sortingMethod = 0)
        {
            List<Image> allImages = new List<Image>();
            List<Image> searchedImages = new List<Image>();
            List<Image> tempImages = new List<Image>();
            int totalPages = searchedImages.Count % resultsPerPage == 0 ? searchedImages.Count / resultsPerPage : (searchedImages.Count / resultsPerPage) + 1;
            foreach (Image image in allImages)
            {
                foreach (ImageTag tag in image.ImageTags)
                {
                    if (tag.Tag == searchString)
                    {
                        searchedImages.Add(image);
                    }
                }
            }
            if (allImages.Count >= resultsPerPage * page)
            {
                tempImages = searchedImages.GetRange((page - 1) * resultsPerPage, resultsPerPage); //if you want 10 results per page, page 1 will have 0-9, page 2 will have 10-19.

            }
            else
            {
                tempImages = searchedImages.GetRange((page - 1) * resultsPerPage, searchedImages.Count % resultsPerPage);
            }
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.Page = page;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.TotalResults = searchedImages.Count;
            return View(tempImages);
        }
        #endregion

        public ActionResult SingleImage(ImageOwner imgOwn)
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
                @ViewBag.LikeCount = ihc.Likes.Where(x => x.ImageID == imgOwn.ImageID && x.OwnerID == imgOwn.OwnerID).Select(x => x).Count();
                imgOwn.Comments = Comments;
                ViewBag.Comments = Comments;
            }
            return View(imgOwn);
        }

        public ActionResult LikeImage(ImageOwner imgOwn)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                if (ihc.Likes.Where(x => x.ImageID == imgOwn.ImageID && x.OwnerID == imgOwn.OwnerID && x.LikerID == WebSecurity.CurrentUserId).Select(x => x).Count() == 0)
                {
                    Like neolike = new Like();
                    neolike.ImageID = imgOwn.ImageID;
                    neolike.OwnerID = imgOwn.OwnerID;
                    neolike.LikerID = WebSecurity.CurrentUserId;
                    neolike.Timestamp = DateTime.Now;
                    ihc.Likes.Add(neolike);
                    ihc.SaveChanges();
                }
               /* ImageOwner image = ihc.ImageOwners.Where(x => x.ImageID == imgOwn.ImageID && x.OwnerID == imgOwn.OwnerID).FirstOrDefault();
                if (image != null)
                {
                    image.Likes = image.Likes + 1;
                    ihc.SaveChanges();
                }*/
            }
            return RedirectToAction("SingleImage", imgOwn);
        }

        public ActionResult BuyImage(ImageOwner image)
        {
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                Member oldOwner = ihc.Members.Where(x => x.MemberID == image.OwnerID).FirstOrDefault();
                Member newOwner = ihc.Members.Where(x => x.MemberID == WebSecurity.CurrentUserId).FirstOrDefault();

                if (image.Price > 0)
                {
                    if (image.OwnerID != newOwner.MemberID && newOwner.AccountBalance >= image.Price)
                    {
                        oldOwner.AccountBalance += image.Price;
                        newOwner.AccountBalance -= image.Price;

                        //Purchase purchase = new Purchase { ImageID = image.ImageID, PurchasePrice = image.Price, PurchaserID = newOwner.MemberID, SellerID = oldOwner.MemberID, TimeOfPurchase = DateTime.Now };
                        //ihc.Purchases.Add(purchase);

                        ImageOwner newImageOwner = new ImageOwner() 
                        {
                            OwnerID = WebSecurity.CurrentUserId, 
                            ImageID = image.ImageID, 
                            Caption = image.Caption, 
                            Title = image.Title, 
                            TimeStamp = image.TimeStamp, 
                            isForSale = false,
                            isAuction = false,
                            Price = 0
                        };

                        ihc.ImageOwners.Add(newImageOwner);
                        ihc.SaveChanges();
                    }

                }
                else
                {
                    throw new Exception("Price is not greater than 0");
                }

                ihc.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddAuction(long startingBid, int auctionDuration, ImageOwner poster)
        {
            DateTime todaysDate = DateTime.Now;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                if (todaysDate < todaysDate.AddDays(auctionDuration))
                {
                    Auction_ auctionToAdd = new Auction_ { CurrentBid = startingBid, ExpirationDate = DateTime.Now.AddDays(auctionDuration), PosterID = poster.OwnerID, ImageID = poster.ImageID };
                    ihc.Auction_.Add(auctionToAdd);
                }
            }
            return View();
        }

        public ActionResult UpdateBid(ImageOwner image, long bid)
        {
            DateTime todaysDate = DateTime.Now;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                Auction_ auction = ihc.Auction_.Where(x => x.ImageID == image.ImageID).FirstOrDefault();
                DateTime? expirationDate = auction.ExpirationDate;
                Member bidder = ihc.Members.Where(x => x.MemberID == WebSecurity.CurrentUserId && x.MemberID != image.OwnerID).FirstOrDefault();

                if (todaysDate < expirationDate)
                {
                    if (bidder.AccountBalance >= image.Price && bidder.AccountBalance >= bid)
                    {
                        auction.CurrentBid = bid;
                        bidder.AccountBalance -= bid;
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}