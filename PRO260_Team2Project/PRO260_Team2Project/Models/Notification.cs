using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace PRO260_Team2Project.Models
{
    public class Notification
    {
        public int PosterID;
        public string userName;
        public DateTime timeStamp;
        public string Content;
        public int ImageID;
        public string title;
        public string type;
        public ImageOwner imgown;

        public Notification(Flag flag)
        {
            PosterID = flag.FlaggerID;
            timeStamp = flag.TimeOfFlag;
            Content = flag.Description;
            ImageID = (int)flag.ImageID;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = Controllers.AccountController.GetNameFromID(PosterID);
            }
            type = "flagNote";
        }
        public Notification(Comment com)
        {
            PosterID = com.PosterID;
            timeStamp = com.TimeStamp;
            Content = com.Content;
            ImageID = com.ImageID;
            type = "CommentNote";
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                imgown = ihc.ImageOwners.Include("Likes1").Include("Auction_").Include("Comments").Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault();
                try
                {

                    userName = Controllers.AccountController.GetNameFromID(PosterID);
                }catch(NullReferenceException nre)
                {
                    userName = "Anonymous";
                }
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }

        public Notification(Like like)
        {
            ImageID = like.ImageID;
            PosterID = like.LikerID;
            type = "LikeNote";
            timeStamp = like.Timestamp;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                imgown = ihc.ImageOwners.Include("Likes1").Include("Auction_").Include("Comments").Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault();
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
                userName = Controllers.AccountController.GetNameFromID(PosterID);
            }
        }

        public Notification(Message message)
        {
            PosterID = message.SenderID;
            timeStamp = DateTime.Now;
            type = "MessageNote";
            Content = message.Content;
            title = message.Title;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = Controllers.AccountController.GetNameFromID(PosterID);
            }
        }

        public Notification(Purchase purch)
        {
            PosterID = purch.PurchaserID;
            ImageID = purch.ImageID;
            timeStamp = purch.TimeOfPurchase;
            Content = "" + purch.PurchasePrice;
            type = "PurchaseNote";
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = Controllers.AccountController.GetNameFromID(PosterID);
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }
    }
}