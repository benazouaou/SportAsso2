﻿using System;
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
    

        public ActionResult Inscription()
        {

            return View();
        }

        public ActionResult Preinscription()
        {
            return View();
        }

        //Gestion de la partie encadrant

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