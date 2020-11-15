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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {

            List<string> list = new List<string>();
            list.Add("female");
            list.Add("male");

            ViewBag.list = new SelectList(list);
            return View();
        }

        [HttpPost]
        public ActionResult Register(Personne p)
        {
            try
            {

                List<string> list = new List<string>();
                list.Add("female");
                list.Add("male");

                ViewBag.list = new SelectList(list);
                Personne pr = db.Personne.Where(x => x.Id_Personne == p.Id_Personne).SingleOrDefault();
                Session["id"] = pr.Id_Personne;


                pr.Id_Personne = p.Id_Personne;
                pr.Nom = p.Nom;
                pr.Prenom = p.Prenom;
                pr.Date_Naissance = p.Date_Naissance;
                pr.E_mail = p.E_mail;
                pr.Num_Telephone = p.Num_Telephone;
                pr.Sexe = p.Sexe;
                pr.Mot_de_Passe = p.Mot_de_Passe;
                pr.Confirm_Mot_Passe = p.Confirm_Mot_Passe;

                db.Personne.Add(pr);
                db.SaveChanges();




                return RedirectToAction("ApresInscription");

            }
            catch (Exception)
            {



                //throw

            }



            return View();
        } // action method end....



        public ActionResult ApresInscription()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
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
            return View();
        }


    }

}