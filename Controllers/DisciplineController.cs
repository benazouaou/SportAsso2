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
                var discipline = context.Discipline
                    .Where(d => d.Nom_Discipline == id)
                    .FirstOrDefault();
                List<Section> sections = context.Section
                    .Where(s => s.Discipline_Id_Discipline == discipline.Id_Discipline)
                    .ToList();
                ViewBag.Sections = sections;
                ViewBag.Discipline = id;
            }
            return View();
        }

        public ActionResult Creneau(string id, int id2)
        {

            using (var context = new Context_db())
            {

                Section section = context.Section
                    .Where(c => c.Id_Section == id2)
                    .FirstOrDefault();
                List<Creneau> creneaux = context.Creneau
                    .Where(c => c.Section_Id_Section == id2)
                    .ToList();
                ViewBag.SectionId = id2;
                ViewBag.Section = section;
                ViewBag.Discipline = id;
                ViewBag.Creneaux = creneaux;
            }
            return View();
        }



      
    }
}