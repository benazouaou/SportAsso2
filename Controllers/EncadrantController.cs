using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportAsso.Models;


namespace SportAsso.Controllers
{
    public class EncadrantController : Controller
    {

        Context_db db = new Context_db();
        // GET: Encadrant
        public ActionResult Index()
        {
            Role rA = db.Role.Where(x => x.Id_Role == 3).SingleOrDefault();
            List<Personne> Encadrants = db.Personne.Where(e => e.Role.Equals(rA)).ToList();

            return View();
        }
    }
}