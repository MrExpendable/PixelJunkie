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
        public ActionResult MessageCenter()
        {
            List<Models.Notification> notes = new List<Models.Notification>();
            notes.AddRange(mc.allComments(WebSecurity.CurrentUserId));
            notes.AddRange(mc.allFlags());
            notes.AddRange(mc.allLikes(WebSecurity.CurrentUserId));
            return View(notes.OrderBy(x => x.timeStamp));
        }

    }
}
