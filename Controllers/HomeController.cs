using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportAsso.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Disciplines()
        {
            ViewBag.Message = "disciplines.";

            return View();
        }

        public ActionResult Encadrants()
        {
            ViewBag.Message = "Encadrant.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Encadrant.";

            return View();
        }
    }
}