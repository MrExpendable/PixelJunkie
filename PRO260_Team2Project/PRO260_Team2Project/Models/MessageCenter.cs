using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace PRO260_Team2Project.Models
{
    public class MessageCenter
    {
        public IEnumerable<Notification> allComments(int userID)
        {
            List<Notification> notes = new List<Notification>();
            using(ImageHolderContext con = new ImageHolderContext())
            {
                foreach(Comment c in con.Comments.Where(x => x.OwnerID == userID).ToList())
                {
                    notes.Add(new Notification(c));
                }
                return notes;
            }

        }
        public IEnumerable<Notification> allFlags()
        {
            List<Notification> notes = new List<Notification>();
            if(Roles.IsUserInRole("Admin"))
            {

                using (ImageHolderContext con = new ImageHolderContext())
                {
                    foreach (Flag f in con.Flags)
                    {
                        notes.Add(new Notification(f));
                    }

                }

            }
            return notes;
        }

        public IEnumerable<Notification> allLikes(int userID)
        {
            List<Notification> notes = new List<Notification>();
            using (ImageHolderContext con = new ImageHolderContext())
            {
                foreach (Like like in con.Likes.Where(x => x.OwnerID == userID).ToList())
                {
                    notes.Add(new Notification(like));
                }

            }
            return notes;
        }

        public IEnumerable<Notification> allMessages(int userID)
        {
            List<Notification> notes = new List<Notification>();
            using (ImageHolderContext con = new ImageHolderContext())
            {
                foreach (Message own in con.Messages.Where(x => x.ReceiverID == userID).ToList())
                {
                    notes.Add(new Notification(own));
                }

            }
            return notes;
        }

        public IEnumerable<Notification> allPurchases(int userID)
        {
            List<Notification> notes = new List<Notification>();
            using (ImageHolderContext con = new ImageHolderContext())
            {
                foreach (Purchase own in con.Purchases.Where(x => x.SellerID == userID).ToList())
                {
                    notes.Add(new Notification(own));
                }

            }
            return notes;
        }


    }
}