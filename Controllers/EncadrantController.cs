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
            //recupere et passe a la vue toutes les personnes ayant le role d'encadrant
            List<Personne> encadrants  = db.Personne.Where(x => x.Role.Any(s => s.Nom_Role == "Encadrant")).ToList();
            ViewBag.Encadrants = encadrants;
            //retourne la vue
            return View();
        }

        public ActionResult Creneaux(int id)
        {
            //recupere l'encadrant dont l'id est identique a celui passe en parametre
            Personne encadrant = db.Personne
                    .Where(e => e.Id_Personne == id)
                    .FirstOrDefault();
            //recupere la liste de tous les creneaux qu'encadre cette personne
            List<Creneau> creneaux = db.Creneau
              .Where(c => c.Encadrant == id)
              .ToList();
            //passe a la vue la liste des creneaux et toutes les infos sur l'encadrant
            ViewBag.Creneaux = creneaux;
            ViewBag.Encadrant = id;
            ViewBag.EncadrantNom = encadrant.Nom;
            ViewBag.EncadrantPrenom = encadrant.Prenom;
            ViewBag.EncadrantMail = encadrant.E_mail;
            //retourne la vue
            return View();
        }
    }
}