using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team2Project.Models;

namespace Team2Project.Controllers
{
    public class MembersDirectoryController : Controller
    {

        //This file is no longer a ghost.
        //
        // GET: /MembersDirectory/
        List<User> users = new List<User>();


        public ActionResult Index(int page, int resultsPerPage)
        {
            ViewBag.Page = page;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.TotalResults = users.Count;
            List<User> tempUsers = new List<User>();
            PopulateTestMembersCollection();
            tempUsers = users.GetRange((page-1) * resultsPerPage, resultsPerPage); //if you want 10 results per page, page 1 will have 0-9, page 2 will have 10-19.
            return View(tempUsers);
        }

        public ActionResult DisplayProfile()
        {
            return View();
        }

        public void PopulateTestMembersCollection()
        {
            ////WHY DO I GET AN ERROR?
            //using(ImageHolderEntities ie = new ImageHolderEntities())
            //{
            //    users = ie.Users.ToList();
            //}
        }
    }
}
