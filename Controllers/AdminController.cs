using SportAsso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportAsso.Controllers
{
    public class AdminController : Controller
    {
        Context_db db = new Context_db();
        // GET: Admin
        public ActionResult Index()
        {
            using (var context = new SportAsso.Models.Context_db())
            {

                if (Session["P_id"] == null)
                {
                    return RedirectToAction("Login");
                }
                int id = (int)Session["P_id"];
                Personne personne = context.Personne
                .Where(p => p.Id_Personne == id)
                .FirstOrDefault();
                ViewBag.Personne = personne;
            }
            return View();
        }


        public ActionResult Recherche()
        {

            return View();
        }

        public ActionResult Preinscription()
        {
            List<SportAsso.Models.Dossier> dossierPre = db.Dossier.Where(d => d.Est_Valide == false).ToList();
            ViewBag.DossiersPre = dossierPre;



            return View();
        }



        public ActionResult Inscription()
        {
            List<SportAsso.Models.Dossier> dossierInsc = db.Dossier.Where(d => d.Est_Valide == true).ToList();
            ViewBag.DossiersPre = dossierInsc;



            return View();
        }


        public ActionResult Detail(int id)
        {

            Dossier d = db.Dossier.Where(s => s.Id_Dossier == id).SingleOrDefault();


            var personne = db.Personne.Where(p => p.Id_Personne == d.Personne_Id_Personne).FirstOrDefault();
            ViewBag.personneNom = personne.Nom;
            ViewBag.personnePrenom = personne.Prenom;



            var section = db.Section.Where(s => s.Id_Section == d.Section_Id_Section).FirstOrDefault();
            ViewBag.section = section.Nom;

            var discipline = db.Discipline.Where(s => s.Id_Discipline == section.Discipline_Id_Discipline).FirstOrDefault();
            ViewBag.discipline = discipline.Nom_Discipline;

            var cr = db.Creneau.Where(c => c.Section_Id_Section == section.Id_Section && (c.Personne1.Any(s => s.Id_Personne == personne.Id_Personne))).SingleOrDefault();
            ViewBag.jour = cr.Jour;
            ViewBag.heure = cr.Heure;

            //récupération des documents du dossier
            List<Document> documents = db.Document
                 .Where(a => a.Dossier_Id_Dossier == d.Id_Dossier)
                 .ToList();

            foreach (Document document in documents as List<Document>)
            {
                switch (document.Type_Document)
                {
                    case "Assurance de responsabilité":
                        ViewBag.Assurance = document;
                        ViewBag.ResID = document.Id_Doc;
                        ViewBag.docValider = document.Est_Valide; 

                        break;
                    case "Attestation medicale":
                        ViewBag.Certificat = document;
                        ViewBag.MedID = document.Id_Doc;
                        ViewBag.MedValide = document.Est_Valide;

                        break;
                    case "Fiche de renseignement":
                        ViewBag.Renseignement = document;
                        ViewBag.FichID = document.Id_Doc;
                        ViewBag.FichValide = document.Est_Valide;

                        break;
                }
                

            }

            return View();
        }


        [HttpPost]

        public ActionResult ValiderDoc(FormCollection cl)
        {



            int id = int.Parse(Request.Form["id"]);
            Document doc = db.Document.Where(s => s.Id_Doc == id).FirstOrDefault();
            doc.Est_Valide = 1;

            ViewBag.id = doc.Dossier_Id_Dossier;
            
            db.SaveChanges();

            return RedirectToAction("/Detail/" + ViewBag.id);
        }
        

        }
}