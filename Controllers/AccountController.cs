using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportAsso.Models;

namespace SportAsso.Controllers
{
    public class AccountController : Controller
    {

        Context_db db = new Context_db();
        // GET: Account
       


        [HttpGet]
        public ActionResult Register( )
        {
            List<string> list = new List<string>();
            list.Add("female");
            list.Add("male");
            ViewBag.list = new SelectList(list);

            Personne p = new Personne();

            return View(p);
        }




        [HttpPost]
        public ActionResult Register(Personne p)
        {
            List<string> list = new List<string>();
            list.Add("female");
            list.Add("male");
            ViewBag.list = new SelectList(list);


            if (db.Personne.Any(x => x.E_mail == p.E_mail))
            {
                ViewBag.DuplicateMessage = "Adresse éléctronique existe déja.";
                return View("Register", p);
            }
            else
            {
                db.Personne.Add(p);
                Role rA = db.Role.Where(x => x.Nom_Role == "Adhérent").SingleOrDefault();
                p.Role.Add(rA);
                db.SaveChanges();

            }

            ModelState.Clear();
            ViewBag.SucessMessage = "Votre compte a était créé avec succés.";
            return View("Register", new Personne());

        }


            [HttpGet]
        public ActionResult Login()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Login(Personne p)
        {
            Personne Pl = db.Personne.Where(x => x.E_mail == p.E_mail && x.Mot_de_Passe == p.Mot_de_Passe).SingleOrDefault();
            if (Pl != null)
            {

                Session["P_id"] = Pl.Id_Personne;
                Session["nom"] = Pl.Nom;
                return RedirectToAction("UserPannel");
            }
            else
            {
                ViewBag.error = "Adresse electronique ou mot de passe invalide";
            }
            return View();
        }




        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login");

        }

//Gestion de l'espace de l'adhérent

        [HttpGet]
        public ActionResult UserPannel()
        {
            if (Session["P_id"] == null)
            {
                return RedirectToAction("Login");
            }
            using (var context = new SportAsso.Models.Context_db())
            {
                int id = (int)Session["P_id"];
                Personne personne = context.Personne
                .Where(p => p.Id_Personne == id)
                .FirstOrDefault();
                ViewBag.Personne = personne;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Inscription(int id)
        {
            using(var context = new Context_db())
            {
                int Id = (int)Session["P_id"];
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id)
                    .FirstOrDefault();
                ViewBag.Dossier = dossier;
                Section section = context.Section
                    .Where(s => s.Id_Section == dossier.Section_Id_Section)
                    .FirstOrDefault();
                ViewBag.Section = section;
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                ViewBag.Discipline = discipline;
                //tous les creneaux auxquels est inscrit l'adhérent
                List<Creneau> creneaux = context.Personne
                    .Where(p => p.Id_Personne == Id)
                    .SelectMany(p => p.Creneau1)
                    .ToList();
                foreach(Creneau creneau in creneaux as List<Creneau>)
                {
                    //le creneau de la section du dossier que l'utilisation consulte
                    if(creneau.Section_Id_Section == section.Id_Section)
                    {
                        ViewBag.Creneau = creneau;
                    }
                }
                Creneau cr = ViewBag.Creneau;
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == cr.Lieu_Id_Lieu)
                    .FirstOrDefault();
                ViewBag.Lieu = lieu;
                Personne encadrant = context.Personne
                    .Where(e => e.Id_Personne == cr.Encadrant)
                    .FirstOrDefault();
                ViewBag.Encadrant = encadrant;
            }
            return View();
        }
        [HttpGet]
        public ActionResult Preinscription(int id)
        {
            using (var context = new Context_db())
            {
                int Id = (int)Session["P_id"];
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id)
                    .FirstOrDefault();
                ViewBag.Dossier = dossier;
                Section section = context.Section
                    .Where(s => s.Id_Section == dossier.Section_Id_Section)
                    .FirstOrDefault();
                ViewBag.Section = section;
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                ViewBag.Discipline = discipline;
                //tous les creneaux auxquels est inscrit l'adhérent
                List<Creneau> creneaux = context.Personne
                    .Where(p => p.Id_Personne == Id)
                    .SelectMany(p => p.Creneau1)
                    .ToList();
                foreach (Creneau creneau in creneaux as List<Creneau>)
                {
                    //le creneau de la section du dossier que l'utilisation consulte
                    if (creneau.Section_Id_Section == section.Id_Section)
                    {
                        ViewBag.Creneau = creneau;
                    }
                }
                Creneau cr = ViewBag.Creneau;
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == cr.Lieu_Id_Lieu)
                    .FirstOrDefault();
                ViewBag.Lieu = lieu;
                Personne encadrant = context.Personne
                    .Where(e => e.Id_Personne == cr.Encadrant)
                    .FirstOrDefault();
                ViewBag.Encadrant = encadrant;
                //récupération des documents du dossier
                List<Document> documents = context.Document
                     .Where(d => d.Dossier_Id_Dossier == dossier.Id_Dossier)
                     .ToList();
                foreach(Document document in documents as List<Document>)
                {
                    switch(document.Type_Document)
                    {
                        case "Assurance de responsabilité civile":
                            ViewBag.Assurance = document;
                            break;
                        case "Attestation medicale":
                            ViewBag.Certificat = document;
                            break;
                        case "Fiche de renseignement":
                            ViewBag.Renseignement = document;
                            break;
                    }
                }
                Session["id_dossier"] = dossier.Id_Dossier;
                Session["S_id"] = section.Id_Section;
            }
            
            return View();
        }

        //Gestion de la partie encadrant
        [HttpGet]
        public ActionResult Cours(int id)
        {
            using(var context = new Context_db())
            {
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .FirstOrDefault();
                ViewBag.Creneau = creneau;
                Section section = context.Section
                    .Where(s => s.Id_Section == creneau.Section_Id_Section)
                    .FirstOrDefault();
                ViewBag.Section = section;
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                ViewBag.Discipline = discipline;
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == creneau.Lieu_Id_Lieu)
                    .FirstOrDefault();
                ViewBag.Lieu = lieu;
                List<Personne> personnes = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .SelectMany(c => c.Personne1)
                    .ToList();
                ViewBag.Personnes = personnes;
                
            }
            return View();
        }


    }

}