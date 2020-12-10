using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportAsso.Models;

namespace SportAsso.Controllers
{
    public class DisciplineController : Controller
    {
        // GET: Discipline
        public ActionResult Index()
        {
            //recupere toutes les disciplines qui existent
            using (var context = new Context_db())
            {
                List<Discipline> disciplines = context.Discipline.ToList();
                ViewBag.Disciplines = disciplines;
            }
            return View();
        }

        public ActionResult Section(string id)
        {
            using (var context = new Context_db())
            {
                //recupere la discipline correspondant a l'id passe en parametre
                var discipline = context.Discipline
                    .Where(d => d.Nom_Discipline == id)
                    .FirstOrDefault();
                //recupere toutes les sections de la discipline
                List<Section> sections = context.Section
                    .Where(s => s.Discipline_Id_Discipline == discipline.Id_Discipline)
                    .ToList();
                //passe tout a la vue
                ViewBag.Sections = sections;
                ViewBag.Discipline = id;
            }
            //retourne la vue correspondant
            return View();
        }

        public ActionResult Creneau(string id, int id2)
        {
            using (var context = new Context_db())
            {
                //recupere la section correspondant a l'id2
                Section section = context.Section
                    .Where(c => c.Id_Section == id2)
                    .FirstOrDefault();
                //recupere tous les creneaux correspondants
                List<Creneau> creneaux = context.Creneau
                    .Where(c => c.Section_Id_Section == id2)
                    .ToList();
                //passe tout a la vue
                ViewBag.SectionId = id2;
                ViewBag.Section = section;
                ViewBag.Discipline = id;
                ViewBag.Creneaux = creneaux;

                //si l'utilisateur est connecte
                if (Session["P_id"] != null)
                {
                    int id_personne = (int)Session["P_id"];
                    //recupere le dossier de l'adherent qui correspond a cette section
                    Dossier dossier = context.Dossier.Where(d => d.Personne_Id_Personne == id_personne && d.Section_Id_Section == section.Id_Section).SingleOrDefault();
                    if (dossier != null)
                    {
                        //si ce dossier existe le passe a la vue
                        ViewBag.dossier = dossier.Id_Dossier;
                    }
                }
            }
            return View();
        }   
    }
}