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
            
            List<Personne> encadrants  = db.Personne.Where(x => x.Role.Any(s => s.Nom_Role == "Encadrant")).ToList();
            ViewBag.Encadrants = encadrants;

      
            return View();
        }

        public ActionResult Creneaux(int id)
        {

            Personne encadrant = db.Personne
                    .Where(e => e.Id_Personne == id)
                    .FirstOrDefault();

            List<Creneau> creneaux = db.Creneau
              .Where(c => c.Encadrant == id)
              .ToList();

            ViewBag.Creneaux = creneaux;
            ViewBag.Encadrant = id;
            ViewBag.EncadrantNom = encadrant.Nom;
            ViewBag.EncadrantPrenom = encadrant.Prenom;

            return View();
        }
    }
}