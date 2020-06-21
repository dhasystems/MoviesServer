using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movies.Hash;
using Movies.Models;


namespace Movies.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register(int id=0)
        {
            User userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Register(User userModel)
        {
            using (DBModels dbModel = new DBModels())
            {
                if (dbModel.Users.Any(x => x.Username == userModel.Username))
                {
                    ViewBag.DuplicateMessage = "Username already exist.";
                    return View("Register", userModel);
                }
                userModel.Password = Hashing.HashPassword(userModel.Password);
                dbModel.Users.Add(userModel);
                dbModel.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";
            return View("Register", new User());

        }
    }


}