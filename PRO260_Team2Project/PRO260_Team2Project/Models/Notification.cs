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
        public string imageTitle;
        public string type;

        public Notification(Flag flag)
        {
            PosterID = flag.FlaggerID;
            timeStamp = flag.TimeOfFlag;
            Content = flag.Description;
            ImageID = (int)flag.ImageID;
            using(ImageHolderContext ihc = new ImageHolderContext())
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
            using(ImageHolderContext ihc = new ImageHolderContext())
            {
                userName = ihc.Members.Where(x => x.MemberID == PosterID).FirstOrDefault().UserName;
                imageTitle = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }

        public Notification(ImageOwner own)
        {
            ImageID = own.ImageID;
            type = "LikeNote";
            timeStamp = DateTime.Now;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                imageTitle = ihc.ImageOwners.Where(x => x.OwnerID == WebSecurity.CurrentUserId && x.ImageID == ImageID).FirstOrDefault().Title;
            }
        }

    }
}