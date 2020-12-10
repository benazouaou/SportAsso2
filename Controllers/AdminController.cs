using SportAsso.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportAsso.Controllers
{
    public class AdminController : Controller
    {
        //cree un context pour interoger la base de donnees via le modele
        Context_db db = new Context_db();
        
        public ActionResult Index()
        {
            using (var context = new SportAsso.Models.Context_db())
            {
                //si l'authentification a echoue, renvoi sur la page de connexion
                if (Session["P_id"] == null)
                {
                    return RedirectToAction("Login");
                }
                //sinon recupere l'id de session
                int id = (int)Session["P_id"];

                //recupere l'admin
                Personne personne = context.Personne
                    .Where(p => p.Id_Personne == id)
                    .FirstOrDefault();

                //envoie la personne a la vue
                ViewBag.Personne = personne;
            }
            //retourne la vue de l'accueil de l'espace admin
            return View();
        }

        public ActionResult Preinscription()
        {
            //recupere tous les dossier non valides
            List<Dossier> dossierPre = db.Dossier
                .Where(d => d.Est_Valide == false)
                .ToList();

            //les passe a la vue
            ViewBag.DossiersPre = dossierPre;

            //retourne la vue reprenant toutes les pre-inscriptions
            return View();
        }

        public ActionResult Inscription()
        {
            //recupere tous les dossiers valides
            List<SportAsso.Models.Dossier> dossierInsc = db.Dossier
                .Where(d => d.Est_Valide == true)
                .ToList();

            //les passe a la vue
            ViewBag.DossiersPre = dossierInsc;

            //retourne la vue reprenant toutes les inscriptions
            return View();
        }

        public ActionResult Detail(int id)
        {
            //recupere et passe a la vue le dossier dont l'id est passe en parametre
            Dossier d = db.Dossier
                .Where(s => s.Id_Dossier == id)
                .SingleOrDefault();
            ViewBag.DossierId = d.Id_Dossier;

            //recupere et passe a la vue le nom et prenom de l'adherent qui posse le dossier
            var personne = db.Personne
                .Where(p => p.Id_Personne == d.Personne_Id_Personne)
                .FirstOrDefault();
            ViewBag.personneNom = personne.Nom;
            ViewBag.personnePrenom = personne.Prenom;

            //recupere et passe a la vuele nom de la section du dossier
            var section = db.Section
                .Where(s => s.Id_Section == d.Section_Id_Section)
                .FirstOrDefault();
            ViewBag.section = section.Nom;

            //recupere et passe a la vue la discipline du dossier
            var discipline = db.Discipline
                .Where(s => s.Id_Discipline == section.Discipline_Id_Discipline)
                .FirstOrDefault();
            ViewBag.discipline = discipline.Nom_Discipline;

            //recupere et passe a la vue le jour et l'heure du creneau
            var cr = db.Creneau
                .Where(c => c.Section_Id_Section == section.Id_Section && (c.Personne1.Any(s => s.Id_Personne == personne.Id_Personne)))
                .SingleOrDefault();
            ViewBag.jour = cr.Jour;
            ViewBag.heure = cr.Heure;

            //recupere les documents du dossier
            List<Document> documents = db.Document
                 .Where(a => a.Dossier_Id_Dossier == d.Id_Dossier)
                 .ToList();

            //parcours ces documents
            foreach (Document document in documents as List<Document>)
            {
                switch (document.Type_Document)
                {
                    //stock le document son id et s'il est valide selon son type
                    case "Assurance de responsabilité civile":
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
            //retourne la vie correspondant au dossier (pre-inscription)
            return View();
        }

        public ActionResult DetailInscription(int id)
        {
            //recupere et passe a la vue le dossier dont l'id est passe en parametre
            Dossier d = db.Dossier
                .Where(s => s.Id_Dossier == id)
                .SingleOrDefault();

            //recupere et passe a la vue le nom et prenom de l'adherent qui posse le dossier
            var personne = db.Personne
                .Where(p => p.Id_Personne == d.Personne_Id_Personne)
                .FirstOrDefault();
            ViewBag.personneNom = personne.Nom;
            ViewBag.personnePrenom = personne.Prenom;

            //recupere et passe a la vuele nom de la section du dossier
            var section = db.Section
                .Where(s => s.Id_Section == d.Section_Id_Section)
                .FirstOrDefault();
            ViewBag.section = section.Nom;

            //recupere et passe a la vue la discipline du dossier
            var discipline = db.Discipline
                .Where(s => s.Id_Discipline == section.Discipline_Id_Discipline)
                .FirstOrDefault();
            ViewBag.discipline = discipline.Nom_Discipline;

            //recupere et passe a la vue le jour et l'heure du creneau
            var cr = db.Creneau
                .Where(c => c.Section_Id_Section == section.Id_Section && (c.Personne1.Any(s => s.Id_Personne == personne.Id_Personne)))
                .SingleOrDefault();
            ViewBag.jour = cr.Jour;
            ViewBag.heure = cr.Heure;

            //recupere les documents du dossier
            List<Document> documents = db.Document
                 .Where(a => a.Dossier_Id_Dossier == d.Id_Dossier)
                 .ToList();

            //les parcours
            foreach (Document document in documents as List<Document>)
            {
                //stock le document son id et s'il est valide selon son type
                switch (document.Type_Document)
                {
                    case "Assurance de responsabilité civile":
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
            //retourne la vie correspondant au dossier (inscription)
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult ValiderDoc(FormCollection cl)
        {
            //recupere l'id du fichier dans le formulaire
            int id = int.Parse(Request.Form["id"]);

            //recupere le document
            Document doc = db.Document
                .Where(s => s.Id_Doc == id)
                .FirstOrDefault();

            //envoie a la vue l'id du dossier correspondant
            ViewBag.id = doc.Dossier_Id_Dossier;

            //valide le document
            doc.Est_Valide = 1;

            //enregistre les modifications dans la abse de donnees
            db.SaveChanges();

            //renvoi l'admin sur la page du dossier
            return RedirectToAction("/Detail/" + ViewBag.id);
        }

        //methode en post
        [HttpPost]
        public ActionResult RefuserDoc(FormCollection cl)
        {
            //recupere l'id du fichier dans le formulaire
            int id = int.Parse(Request.Form["id"]);

            //recupere le document
            Document doc = db.Document
                .Where(s => s.Id_Doc == id)
                .FirstOrDefault();

            //envoie a la vue l'id du dossier correspondant
            ViewBag.id = doc.Dossier_Id_Dossier;

            //recupere le chemin d'acces au fichier 
            string path = doc.Path;

            //supprime le fichier stocke
            System.IO.File.Delete(path);

            //supprime la ligne du fichier dans la base de donnees
            db.Document.Remove(doc);

            //sauvegarde les modifications dans la base de donnees
            db.SaveChanges();

            //renvoi l'admin sur la page du dossier
            return RedirectToAction("/Detail/" + ViewBag.id);
        }

        //methode en post
        [HttpPost]
        public FileResult Visualiser(FormCollection cl)
        {
            int id = int.Parse(Request.Form["id"]);

            //recupere le document dont l'id est passe via le formulaire
            Document doc = db.Document
                .Where(d => d.Id_Doc == id)
                .FirstOrDefault();

            //recupere le chemin du fichier, son id et son type
             string path = doc.Path;
             string fileName = doc.Type_Document+doc.Id_Doc;

            //recupere le fichier sous forme de tableau de byte
            byte[] fileByte = System.IO.File.ReadAllBytes(path);

            //retourne le fichier recupere
            return File(fileByte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        //methode en post
        [HttpPost]
        public ActionResult ValiderDossier(FormCollection cl)
        {
            //recupere l'id du dossier dans le formulaire
            int id = int.Parse(Request.Form["id"]);

            //recupere le dossier correspondant
            Dossier d = db.Dossier
                .Where(dos => dos.Id_Dossier == id)
                .FirstOrDefault();

            //le valide et enregistre le changement dans la base de donnees
            d.Est_Valide = true;
            db.SaveChanges();

            //renvoie la page d'accueil de l'espace admin
            return Redirect("/Admin");
        }
    }
}