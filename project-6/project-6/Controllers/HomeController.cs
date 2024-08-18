using project_6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_6.Controllers
{
    public class HomeController : Controller
    {
        private MiniProjectEntities db = new MiniProjectEntities();
        public ActionResult Index()
        {
            //db.Categories.ToList()
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}