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

//partie connexion et inscription

        //cree un context pour interoger la base de donnees via le modele
        Context_db db = new Context_db();
        // GET: Account
       
        //methode en get
        [HttpGet]
        public ActionResult Register( )
        {
            //cree une liste avec les possibilite de sexe a saisir pour s'inscrire
            List<string> list = new List<string>();
            list.Add("female");
            list.Add("male");

            //passe la liste a la vue
            ViewBag.list = new SelectList(list);

            //cree un personne
            Personne p = new Personne();

            //retourne la vue permettant de s'inscrire en lui passant la personne
            return View(p);
        }

        //methode en post
        [HttpPost]
        public ActionResult Register(Personne p)
        {
            //cree une liste avec les possibilite de sexe a saisir pour s'inscrire
            List<string> list = new List<string>();
            list.Add("female");
            list.Add("male");

            //passe la liste a la vue
            ViewBag.list = new SelectList(list);

            //si l'adresse mail est deja prise par une personne dans la base de donnees
            if (db.Personne.Any(x => x.E_mail == p.E_mail))
            {
                //passe un message d'erreur a la vue permettant de s'inscrire et retourne cette vue
                ViewBag.DuplicateMessage = "Adresse éléctronique existe déja.";
                return View("Register", p);
            }
            else
            {
                //sinon ajoute la personne rempli dans la vue register a la base de donnees
                db.Personne.Add(p);
                //ajoute le role adherent a cette personne
                Role rA = db.Role.Where(x => x.Nom_Role == "Adhérent").SingleOrDefault();
                p.Role.Add(rA);
                //enregistre les changements de la base de donnees
                db.SaveChanges();
            }

            ModelState.Clear();
            //passe a la vue un message de confirmation et retourne la vue
            ViewBag.SucessMessage = "Votre compte a était créé avec succés.";
            return View("Register", new Personne());
        }

        //methode en get
        [HttpGet]
        public ActionResult Login()
        {
            //retourne la vue permettant de se connecter
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult Login(Personne p)
        {
            //recupere la personne dont l'email et le mot de passe correspondent a ceux de la personne en parametre
            Personne Pl = db.Personne
                .Where(x => x.E_mail == p.E_mail && x.Mot_de_Passe == p.Mot_de_Passe)
                .SingleOrDefault();
            
            //si cette personne existe
            if (Pl != null)
            {
                //initialise la Session avec son id et son nom
                Session["P_id"] = Pl.Id_Personne;
                Session["nom"] = Pl.Nom;

                //si cette personne a le role admin 
                if (Pl.Role.Any(s => s.Nom_Role == "Admin"))
                {
                    //enregistre l'information dans la session et redirige l'utilisateur vers son espace admin
                    Session["Admin"] = true;
                    return Redirect("/Admin/Index");
                }
                else
                {
                    //sinon le redirige vers son espace adherent
                    return RedirectToAction("UserPannel");
                }               
            }
            else
            {
                //sinon passe un message d'erreur a la vue et redirige l'utilisateur vers l'espace de connexion pour qu'il retente de se connecter
                ViewBag.error = "Adresse electronique ou mot de passe invalide";
            }
            return View();
        }

        //methode en post
        [HttpGet]
        public ActionResult Logout()
        {
            //vide la session et redirige l'utilisateur vers la page de connexion
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }



//partie de l'espace de l'adhérent

        //methode en get
        [HttpGet]
        public ActionResult UserPannel()
        {
            //si l'id de l'utilisateur n'est pas initialise dans la session
            if (Session["P_id"] == null)
            {
                //refdirige vers la page de connexion
                return RedirectToAction("Login");
            }
            //sinon recupere les informations concernant l'adherent
            using (var context = new SportAsso.Models.Context_db())
            {
                int id = (int) Session["P_id"];

                //recupere l'adherent a l'aide de son id stocke dans session
                Personne personne = context.Personne
                    .Where(p => p.Id_Personne == id)
                    .FirstOrDefault();

                //passe l'info a la vue
                ViewBag.Personne = personne;
            }
            //retourne la vue de la page des infos personnelles de l'espace adherent
            return View();
        }

        //methode en get
        [HttpGet]
        public ActionResult Inscription(int id)
        {
            //recupere les informations concernant l'inscription a afficher
            using(var context = new Context_db())
            {
                int Id = (int)Session["P_id"];

                //recupere le dossier dont l'id est passe en parametre de la methode
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Dossier = dossier;

                //recupere la section concernee par le dossier
                Section section = context.Section
                    .Where(s => s.Id_Section == dossier.Section_Id_Section)
                    .FirstOrDefault();
                //la passe a la vue
                ViewBag.Section = section;

                //recupere la discipline correspondant a la section
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                //la passe a la vue
                ViewBag.Discipline = discipline;


                //recupere tous les creneaux auxquels est inscrit l'adhérent
                List<Creneau> creneaux = context.Personne
                    .Where(p => p.Id_Personne == Id)
                    .SelectMany(p => p.Creneau1)
                    .ToList();

                //parcours ces creneaux
                foreach(Creneau creneau in creneaux as List<Creneau>)
                {
                    //si c'est le creneau de la section du dossier que l'utilisation consulte
                    if(creneau.Section_Id_Section == section.Id_Section)
                    {
                        //passe ce creneau a la vue
                        ViewBag.Creneau = creneau;
                    }
                }

                //recupere le creneau passe a la vue
                Creneau cr = ViewBag.Creneau;
                //recupere le lieu correspondant a ce creneau
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == cr.Lieu_Id_Lieu)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Lieu = lieu;

                //recupere l'encadrant de ce creneau
                Personne encadrant = context.Personne
                    .Where(e => e.Id_Personne == cr.Encadrant)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Encadrant = encadrant;
            }
            //retourne la vue correspondant au dossier valide passe en parametre
            return View();
        }

        //methode en get
        [HttpGet]
        public ActionResult Preinscription(int id)
        {
            //recupere les informations concernant la pre-inscription a afficher
            using (var context = new Context_db())
            {
                int Id = (int)Session["P_id"];

                //recupere le dossier dont l'id est passe en parametre de la methode
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Dossier = dossier;

                //recupere la section correspondant a ce dossier
                Section section = context.Section
                    .Where(s => s.Id_Section == dossier.Section_Id_Section)
                    .FirstOrDefault();
                //la passe a la vue
                ViewBag.Section = section;

                //recupere la discipline de cette section
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                //la passe a la vue
                ViewBag.Discipline = discipline;


                //recupere tous les creneaux auxquels est inscrit l'adhérent
                List<Creneau> creneaux = context.Personne
                    .Where(p => p.Id_Personne == Id)
                    .SelectMany(p => p.Creneau1)
                    .ToList();

                //parcours ces creneaux
                foreach (Creneau creneau in creneaux as List<Creneau>)
                {
                    //si c'est le creneau de la section du dossier que l'utilisation consulte
                    if (creneau.Section_Id_Section == section.Id_Section)
                    {
                        //passe ce creneau a la vue
                        ViewBag.Creneau = creneau;
                    }
                }

                //recupere le creneau passe a la vue
                Creneau cr = ViewBag.Creneau;
                //recupere le lieu correspondant a ce creneau
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == cr.Lieu_Id_Lieu)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Lieu = lieu;

                //recupere l'encadrant de ce creneau
                Personne encadrant = context.Personne
                    .Where(e => e.Id_Personne == cr.Encadrant)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Encadrant = encadrant;


                //recupere les documents du dossier
                List<Document> documents = context.Document
                     .Where(d => d.Dossier_Id_Dossier == dossier.Id_Dossier)
                     .ToList();

                //parcours ces documents
                foreach(Document document in documents as List<Document>)
                {
                    //selon le type du document
                    switch(document.Type_Document)
                    {
                        //si c'est l'assurance de responsabilite civiel
                        case "Assurance de responsabilité civile":
                            //le passe a la vue sous le nom Assurannce
                            ViewBag.Assurance = document;
                            break;
                        //si c'est l'attestation medicale
                        case "Attestation medicale":
                            //le passe a la vue sous le nom de Certificat
                            ViewBag.Certificat = document;
                            break;
                        //si c'est la fiche de renseignement
                        case "Fiche de renseignement":
                            //le passe a la vue sous le nom de Renseignement
                            ViewBag.Renseignement = document;
                            break;
                    }
                }
                //stock l'id du dossier et de la section dans la session
                Session["id_dossier"] = dossier.Id_Dossier;
                Session["S_id"] = section.Id_Section;
            }
            //retourne la vue correspondant au dossier dont l'id est passe en parametre
            return View();
        }

//Gestion de la partie encadrant

        //methode en get
        [HttpGet]
        public ActionResult Cours(int id)
        {
            //recupere les donnees necessaires pour la page reprenant un creneau d'un encadrant
            using(var context = new Context_db())
            {
                //recupere et passe a la vue le creneau dont l'id est passe en parametre
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .FirstOrDefault();
                ViewBag.Creneau = creneau;

                //recupere et passe a la vue la section de ce creneau
                Section section = context.Section
                    .Where(s => s.Id_Section == creneau.Section_Id_Section)
                    .FirstOrDefault();
                ViewBag.Section = section;

               //recupere et passe a la vue la discipline de cette section
                Discipline discipline = context.Discipline
                    .Where(d => d.Id_Discipline == section.Discipline_Id_Discipline)
                    .FirstOrDefault();
                ViewBag.Discipline = discipline;

                //recupere et passe a la vue le lieu du creneau
                Lieu lieu = context.Lieu
                    .Where(l => l.Id_Lieu == creneau.Lieu_Id_Lieu)
                    .FirstOrDefault();
                ViewBag.Lieu = lieu;

                //recupere et passe a la vue les personnes inscrites a ce creneau
                List<Personne> personnes = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .SelectMany(c => c.Personne1)
                    .ToList();
                ViewBag.Personnes = personnes;             
            }
            //retourne la vue reprenant le creneau de l'encadrant
            return View();
        }


    }
}