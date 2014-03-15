using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRO260_Team2Project.Models;
using WebMatrix.WebData;

namespace PRO260_Team2Project.Controllers
{
    public class MessageCenterController : Controller
    {
        //
        // GET: /MessageCenter/
        MessageCenter mc = new MessageCenter();
        [Authorize]
        public ActionResult MessageCenter()
        {
            List<Models.Notification> notes = new List<Models.Notification>();
            notes.AddRange(mc.allComments(WebSecurity.CurrentUserId));
            notes.AddRange(mc.allFlags());
            notes.AddRange(mc.allLikes(WebSecurity.CurrentUserId));
            notes.AddRange(mc.allMessages(WebSecurity.CurrentUserId));
            notes.AddRange(mc.allPurchases(WebSecurity.CurrentUserId));
            return View(notes.OrderByDescending(x => x.timeStamp));
        }

        [Authorize]
        [HttpPost]
        public ActionResult sendMessage(String username, string title, string messageText)
        {
            Message m = new Message();
            m.ReceiverID = WebSecurity.GetUserId(username);
            m.Title = title;
            m.Content = messageText;
            m.SenderID = WebSecurity.CurrentUserId;
            m.TimeStamp = DateTime.Now;
            using (ImageHolderContext ihc = new ImageHolderContext())
            {
                ihc.Messages.Add(m);
                ihc.SaveChanges();
            }
            return Redirect("MessageCenter");
        }

    }
}
