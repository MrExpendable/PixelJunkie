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

        public Notification(Flag flag)
        {
            PosterID = flag.FlaggerID;
            timeStamp = flag.TimeOfFlag;
            Content = flag.Description;
            ImageID = (int)flag.ImageID;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = ihc.Members.Where(x => x.MemberID == PosterID).FirstOrDefault().UserName;
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
                userName = ihc.Members.Where(x => x.MemberID == PosterID).FirstOrDefault().UserName;
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }

        //Like
        public Notification(ImageOwner own)
        {
            ImageID = own.ImageID;
            type = "LikeNote";
            timeStamp = DateTime.Now;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }

        public Notification(Message message)
        {
            PosterID = WebSecurity.CurrentUserId;
            timeStamp = DateTime.Now;
            type = "MessageNote";
            Content = message.Content;
            title = message.Title;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = ihc.Members.Where(x => x.MemberID == PosterID).FirstOrDefault().UserName;
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
                userName = ihc.Members.Where(x => x.MemberID == PosterID).FirstOrDefault().UserName;
                title = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }
    }
}