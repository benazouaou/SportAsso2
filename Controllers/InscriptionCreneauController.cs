using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SportAsso.Models;
using System.Net;

namespace SportAsso.Controllers
{
    public class InscriptionCreneauController : Controller

    {
        Context_db context = new Context_db();
        // GET: InscriptionCreneau

        public ActionResult Index(int id)
        {
           

            Creneau creneau = context.Creneau.Where(x => x.Id_Creneau == id).FirstOrDefault();
            ViewBag.Creneau = creneau;
            ViewBag.idCreneau = creneau.Id_Creneau;
            Session["Creneau"] = creneau.Id_Creneau;

            Section s = context.Section.Where(x => x.Id_Section == creneau.Section_Id_Section).FirstOrDefault();
            ViewBag.SectionId = s.Id_Section;
            ViewBag.SectionCreneau = s.Nom;
            ViewBag.PrixSection = s.Prix;
            Session["S_id"]= s.Id_Section;



            Lieu l = context.Lieu.Where(x => x.Id_Lieu == creneau.Lieu_Id_Lieu).FirstOrDefault();
            ViewBag.LieuCreneau = l.Nom;
            ViewBag.AdresseCreneau = l.Adresse;

            Personne p = context.Personne.Where(x => x.Id_Personne == creneau.Encadrant).FirstOrDefault();
            ViewBag.Encadrant = p;

            Discipline d = context.Discipline.Where(x => x.Id_Discipline == s.Discipline_Id_Discipline).FirstOrDefault();
            ViewBag.CreneauDiscipline = d.Nom_Discipline;

            

            return View();
        }

      
       

            public ActionResult Confirmation( string id, string id2)
            {
                ViewBag.Discipline = id;
                ViewBag.Section = id2;

                

            return View();

            }


       


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // permet d'empêcher les attaques de falsification de requêtes intersites
        public ActionResult Confirmation(HttpPostedFileBase[] nameFile)
        {
             Dossier d = new Dossier();
             d.Section_Id_Section = (int)Session["S_id"];
             d.Personne_Id_Personne = (int)Session["P_id"];
             d.Paiement = 1;
             context.Dossier.Add(d);
             context.SaveChanges();

            for (int i = 0; i < 3; i++)
            {
                if (nameFile[i] != null)
                    try
                    {
                        string typeDoc;
                        string fileExtension = System.IO.Path.GetExtension(nameFile[i].FileName);
                        if (i == 0)
                        {
                            typeDoc = "Fiche de renseignement";
                        }
                        else if (i == 1)
                        {
                            typeDoc = "Attestation medicale";                           
                        }
                        else
                        {
                            typeDoc = "Assurance de responsabilité civile";                            
                        }

                        //l'ajout dans le dossier Files
                        string name = Session["P_id"] + "_" + Session["S_id"] + "_" + typeDoc + fileExtension;

                        string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(nameFile[i].FileName.Replace(nameFile[i].FileName, name)));

                        nameFile[i].SaveAs(path);

                        //l'ajout dans la base de données
                        Document doc = new Document();
                        doc.Dossier_Id_Dossier = d.Id_Dossier;
                        doc.Type_Document = typeDoc;
                        doc.Path = path;
                        doc.Est_Valide = 0;
                        context.Document.Add(doc);
                        context.SaveChanges();

                    }

                    catch (Exception ex)
                    {
                        ViewBag.message = "Erreur !" + ex;
                    }            
            }

            ViewBag.length = nameFile.Length;

            //decrementer le nombre de places disponible 
            int id = (int)Session["Creneau"];
            Creneau creneau = context.Creneau.Where(x => x.Id_Creneau == id).FirstOrDefault();
            creneau.Nombre_Places_Dispo -= 1;
            context.SaveChanges();

            return View();

        }


        public FileResult Telecharger(string fileName)
        {
            fileName = "Fiche de renseignement.pdf";
            string path = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileByte = System.IO.File.ReadAllBytes(path);
            return File(fileByte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
    }
}

