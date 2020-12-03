using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SportAsso.Models;

namespace SportAsso.Controllers
{
    public class InscriptionCreneauController : Controller
       
    {
        Context_db context = new Context_db();
        // GET: InscriptionCreneau
       
        public ActionResult Index(int id)
        {
            Creneau creneau = context.Creneau.Where(x => x.Id_Creneau == id).FirstOrDefault();
            ViewBag.Creneau = creneau.Id_Creneau;
            Section s =  context.Section.Where(x => x.Id_Section == creneau.Section_Id_Section).FirstOrDefault();
            ViewBag.SectionCreneau = s.Nom;
            Discipline d = context.Discipline.Where(x => x.Id_Discipline == s.Discipline_Id_Discipline).FirstOrDefault();
            ViewBag.CreneauDiscipline = d.Nom_Discipline;

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // permet d'empêcher les attaques de falsification de requêtes intersites
        public ActionResult Index(HttpPostedFileBase nameFile)
        {
            if (nameFile != null && nameFile.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(nameFile.FileName));
                    nameFile.SaveAs(path);
                    ViewBag.message = "Envoyé."; ;
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Erreur !" + ex;
                }
            else
            {
                ViewBag.message = "Vous devez sélectionner un fichier.";
            }
            return View();
        }





        public ActionResult payerCreneau(int id)
        {
            Creneau creneau = context.Creneau.Where(x => x.Id_Creneau == id).FirstOrDefault();
            ViewBag.Creneau = creneau;
            ViewBag.idCreneau = creneau.Id_Creneau;

            Section s = context.Section.Where(x => x.Id_Section == creneau.Section_Id_Section).FirstOrDefault();
            ViewBag.SectionCreneau = s.Nom;
            ViewBag.PrixSection = s.Prix;

            Lieu l = context.Lieu.Where(x => x.Id_Lieu == creneau.Lieu_Id_Lieu).FirstOrDefault();
            ViewBag.LieuCreneau = l.Nom;
            ViewBag.AdresseCreneau = l.Adresse;
            
            Personne p = context.Personne.Where(x => x.Id_Personne == creneau.Encadrant).FirstOrDefault();
            ViewBag.Encadrant = p;

            Discipline d = context.Discipline.Where(x => x.Id_Discipline == s.Discipline_Id_Discipline).FirstOrDefault();
            ViewBag.CreneauDiscipline = d.Nom_Discipline;

            return View();
        }


        public ActionResult Confirmation(string id, string id2)
        {
            ViewBag.Discipline = id;
            ViewBag.Section = id2;
            

            return View();

        }

        }
    }
