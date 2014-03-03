using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeek2.Models;

namespace TestWeek2.Controllers
{
    public class HomeController : Controller
    {
        Image image = new Image();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult Featured()
        {
            return View(image);
        }

        public ActionResult Image()
        {
            return View(image);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }
    }
}
