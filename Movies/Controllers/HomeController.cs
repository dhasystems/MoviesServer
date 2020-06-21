using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movies.Models;
using System.Linq.Dynamic;
namespace Movies.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetList()
        {
            // Server side parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<List> movList = new List<List>();
            using (DBModels db = new DBModels())
            {
                movList = db.Lists.ToList<List>();
                int totalrows = movList.Count;

                if (!string.IsNullOrEmpty(searchValue)) // filter
                {
                    movList = movList.
                        Where(x => x.title.ToLower().Contains(searchValue.ToLower()) ||
                        x.year.ToLower().Contains(searchValue.ToLower()) ||
                        x.tmdb_id.Value.ToString().Contains(searchValue.ToLower()) ||
                        x.genre.ToString().Contains(searchValue.ToLower()) ||
                        x.quality.ToString().Contains(searchValue.ToLower()) ||
                        x.type.ToString().Contains(searchValue.ToLower())).ToList<List>();
                }

                int totalrowsafterfiltering = movList.Count;

                //sorting
                movList = movList.OrderBy(sortColumnName + " " + sortDirection).ToList<List>();

                //paging
                movList = movList.Skip(start).Take(length).ToList<List>();

                return Json(new { data = movList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

            }



        }
    }
}