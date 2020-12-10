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
            //retourne la vue de la page d'accueil du site
            return View();
        }

        public ActionResult Contact()
        {
            //retourne la vue de la page contact du site
            return View();
        }
    }
}