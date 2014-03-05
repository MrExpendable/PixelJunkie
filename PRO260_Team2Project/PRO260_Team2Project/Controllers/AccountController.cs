using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using PRO260_Team2Project.Models;
using PRO260_Team2Project.Models.AccountModels;

namespace PRO260_Team2Project.Controllers
{
    public class AccountController : Controller
    {
        List<User> users = new List<User>();


        #region index
        public ActionResult Index(int page = 1, int resultsPerPage = 3, int sortingMethod = 0)
        {
            List<User> tempUsers = new List<User>();
            UpdateUsers();

            int totalPages = users.Count % resultsPerPage == 0 ? users.Count / resultsPerPage : users.Count / resultsPerPage + 1;
            if (users.Count >= resultsPerPage * page)
            {
                tempUsers = users.GetRange((page - 1) * resultsPerPage, resultsPerPage); //if you want 10 results per page, page 1 will have 0-9, page 2 will have 10-19.
            }
            else
            {
                tempUsers = users.GetRange((page - 1) * resultsPerPage, users.Count % resultsPerPage);
            }
            if (sortingMethod == 0)
            {
                //tempUsers = tempUsers.OrderBy(x => x.Score).ToList();
                tempUsers = tempUsers.OrderBy(x => x.UserName).ToList();
            }
            else
            {
                tempUsers = tempUsers.OrderBy(x => x.UserName).ToList();
            }
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.TotalResults = users.Count;
            return View(tempUsers);
        }
        #endregion

        #region Search
        public ActionResult Search(String searchString = "", int page = 1, int resultsPerPage = 3, int sortingMethod = 0)
        {
            List<User> searchedUsers = new List<User>();
            List<User> tempUsers = new List<User>();
            UpdateUsers();
            int totalPages = searchedUsers.Count % resultsPerPage == 0 ? searchedUsers.Count / resultsPerPage : (searchedUsers.Count / resultsPerPage) + 1;
            foreach (User user in users)
            {
                if (user.UserName.Contains(searchString))
                {
                    searchedUsers.Add(user);
                }
            }
            if (users.Count >= resultsPerPage * page)
            {
                tempUsers = searchedUsers.GetRange((page - 1) * resultsPerPage, resultsPerPage); //if you want 10 results per page, page 1 will have 0-9, page 2 will have 10-19.

            }
            else
            {
                tempUsers = searchedUsers.GetRange((page - 1) * resultsPerPage, searchedUsers.Count % resultsPerPage);
            }
            if (sortingMethod == 0)
            {
                //tempUsers = tempUsers.OrderBy(x => x.Score).ToList();
                tempUsers = tempUsers.OrderBy(x => x.UserName).ToList();
            }
            else
            {
                tempUsers = tempUsers.OrderBy(x => x.UserName).ToList();
            }
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.Page = page;
            ViewBag.ResultsPerPage = resultsPerPage;
            ViewBag.TotalResults = searchedUsers.Count;
            return View(tempUsers);
        }
        #endregion

        #region UserManipulation
        private void UpdateUsers()
        {
            using (MembershipContext ie = new MembershipContext())
            {
                users = ie.Users.ToList();
            }
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = false; //OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = false;//OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Login

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect. Or maybe you're just a bad person.");
            return View(model);
        }

        #endregion

        #region Logout

        [HttpGet]
        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return Redirect("~/");
        }

        #endregion

        #region Registration

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                // OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}