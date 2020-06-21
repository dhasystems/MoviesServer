using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movies.Models;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModels db = new DBModels())
            {
                List<List> movList = db.Lists.ToList<List>();
                return Json(new { data = movList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new List());
            else
            {
                using (DBModels db = new DBModels())
                {
                    return View(db.Lists.Where(x => x.id == id).FirstOrDefault<List>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(List mov)
        {
            using (DBModels db = new DBModels())
            {
                if (mov.id == 0)
                {
                    db.Lists.Add(mov);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    
                }
                else
                {
                    db.Entry(mov).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModels db = new DBModels())
            {
                List mov = db.Lists.Where(x => x.id == id).FirstOrDefault<List>();
                db.Lists.Remove(mov);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogOut()
        {
            //Session["username"] = null;
            Session.Clear();
            return RedirectToAction("Index", "Home");

        }

    }
}