using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movies.Models;
using Movies.Hash;

namespace Movies.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Authorize(int id=0)
        {
            User userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Authorize(User userModel)
        {
            using (DBModels dbModel = new DBModels())
            {
                var currentAccount = dbModel.Users.SingleOrDefault(a => a.Username.Equals(userModel.Username));
                if (currentAccount != null)
                {
                    if (Hashing.ValidatePassword(userModel.Password, currentAccount.Password))
                    {
                        //Session.Add("username", account.username);
                        Session["username"] = currentAccount.Username;
                        return RedirectToAction("Index", "Movies");
                    }
                }
                userModel.LoginErrorMessage = "Wrong username or password.";
                return View("Authorize", userModel);
            }
        }

      
    }
}