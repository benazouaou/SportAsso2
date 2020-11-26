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
                Role rA = db.Role.Where(x => x.Id_Role == 1).SingleOrDefault();
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



        [HttpGet]
        public ActionResult UserPannel()
        {
            if (Session["P_id"] == null)
            {
                return RedirectToAction("Login");
            }
            int id = (int)Session["P_id"];
            using (var context = new Context_db())
            {
                List<Role> roles = context.Personne
                    .Where(p => p.Id_Personne == id)
                    .SelectMany(p => p.Role)
                    .ToList();
                ViewBag.Roles = roles;

                List<Creneau> creneaux = context.Personne
                    .Where(p => p.Id_Personne == id)
                    .SelectMany(p => p.Creneau)
                    .ToList();
                ViewBag.Creneaux = creneaux;

                List<Dossier> dossier = context.Dossier
                        .Where(d => d.Personne_Id_Personne == id)
                        .ToList();
                ViewBag.Dossiers = dossier;
            }
            return View();


        }



    }

}