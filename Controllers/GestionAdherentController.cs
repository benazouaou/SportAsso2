using System;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web;
using SportAsso.Models;
using System.Collections.Generic;
using System.IO;


namespace SportAsso.Controllers
{
    public class GestionAdherentController : Controller
    {
        // GET: GestionAdherent
        public ActionResult Index()
        {
            return RedirectToRoute("/Account/UserPannel");
        }

        [HttpGet]
        public ActionResult ModifierMdp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ModifierMdp(FormCollection id)
        {
            var ancien = Request.Form["ancien"];
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            int Id = (int)Session["P_id"];
            var error = false;
            using (var context = new Context_db())
            {
                Personne mdp = context.Personne
                      .Where(p => p.Id_Personne == Id)
                      .FirstOrDefault();
                ViewBag.mdp = mdp.Mot_de_Passe;
                if (ancien != ViewBag.mdp)
                {
                    error = true;
                    ViewBag.Ancien = "Mot de passe erroné";
                }
                if (nouveau != nouveauConf || nouveau == "")
                {
                    error = true;
                    ViewBag.Nouveau = "Les deux mots de passe sont différents ou vides";
                }
                if (nouveau.Length < 4)
                {
                    error = true;
                    ViewBag.Nouveau = "Nouveau mot de passe trop court (au moins 4 caractères)";
                }
            }
            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        personne.Mot_de_Passe = (string)nouveau;
                        personne.Confirm_Mot_Passe = (string)nouveau;
                        context.SaveChanges();
                        return Redirect("/Account/UserPannel");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        Response.Write(nouveau);
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult ModifierMail()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ModifierMail(FormCollection id)
        {
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            int Id = (int)Session["P_id"];
            var error = false;

            if (nouveau != nouveauConf || nouveau == "")
            {
                error = true;
                ViewBag.Nouveau = "Les deux mots de passe sont différents ou vides";
            }

            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        personne.E_mail = (string)nouveau;
                        context.SaveChanges();
                        return Redirect("/Account/UserPannel");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        Response.Write(nouveau);
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult ModifierTel()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ModifierTel(FormCollection id)
        {
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            int Id = (int)Session["P_id"];
            var error = false;

            if (nouveau != nouveauConf || nouveau == "")
            {
                error = true;
                ViewBag.Nouveau = "Les deux numéros de téléphone sont différents ou vides";
            }

            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        personne.Num_Telephone = (string)nouveau;
                        context.SaveChanges();
                        return Redirect("/Account/UserPannel");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        Response.Write(nouveau);
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ModifierCreneau(int id)
        {
            using (var context = new Context_db())
            {
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .FirstOrDefault();
                ViewBag.Creneau = creneau;
                List<Creneau> creneaux = context.Creneau
                   .Where(c => c.Section_Id_Section == creneau.Section_Id_Section)
                   .ToList();
                ViewBag.Creneaux = creneaux;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ModifierCreneau(FormCollection id)
        {
            var ancien = int.Parse(Request.Form["ancien"]);
            var nouveau = int.Parse(Request.Form["nouveau"]);
            int Id = (int)Session["P_id"];
            using (var context = new Context_db())
            {
                try
                {
                    Creneau ancienC = context.Creneau
                        .Where(c => c.Id_Creneau == ancien)
                        .FirstOrDefault();
                    Creneau nouveauC = context.Creneau
                        .Where(c => c.Id_Creneau == nouveau)
                        .FirstOrDefault();
                    nouveauC.Nombre_Places_Dispo = nouveauC.Nombre_Places_Dispo - 1;
                    ancienC.Nombre_Places_Dispo = ancienC.Nombre_Places_Dispo + 1;
                    Personne adherent = context.Personne
                        .Where(p => p.Id_Personne == Id)
                        .FirstOrDefault();
                    adherent.Creneau1.Add(nouveauC);
                    adherent.Creneau1.Remove(ancienC);
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {

                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            return Redirect("/Account/UserPannel");
        }
        [HttpPost]
        public ActionResult SupprimerDossier(FormCollection id)
        {
            var dossierId = int.Parse(Request.Form["dossier"]);
            var creneauId = int.Parse(Request.Form["creneau"]);
            using (var context = new Context_db())
            {
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == dossierId)
                    .FirstOrDefault();
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == creneauId)
                    .FirstOrDefault();
                Personne adherent = context.Personne
                    .Where(p => p.Id_Personne == dossier.Personne_Id_Personne)
                    .FirstOrDefault();
                try
                {
                    context.Dossier.Remove(dossier);
                    adherent.Creneau1.Remove(creneau);
                    creneau.Nombre_Places_Dispo = creneau.Nombre_Places_Dispo + 1;
                    context.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {

                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            return Redirect("/Account/UserPannel");
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // permet d'empêcher les attaques de falsification de requêtes intersites
        public ActionResult ModificationDoc()
        {
            HttpPostedFileBase responsabilite = Request.Files["responsabilite"];
            HttpPostedFileBase renseignement = Request.Files["renseignement"];
            HttpPostedFileBase medical = Request.Files["medical"];
            using (var context = new Context_db())
            {
                //récupérer le nom du dossier
                int id_dossier = (int) Session["id_dossier"];
                int Id = (int) Session["P_id"];
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id_dossier)
                    .FirstOrDefault();

                //créer le document 
                try
                {
                string typeDoc;
                string fileExtension;
                HttpPostedFileBase nameFile;
                    if (renseignement != null)
                    {
                        typeDoc = "Fiche de renseignement";
                        fileExtension = Path.GetExtension(renseignement.FileName);
                        nameFile = renseignement;
                    }
                    else if (medical != null)
                    {
                        typeDoc = "Attestation medicale";
                        fileExtension = Path.GetExtension(medical.FileName);
                        nameFile = medical;
                    }
                    else
                    {
                        typeDoc = "Assurance de responsabilité civile";
                        fileExtension = Path.GetExtension(responsabilite.FileName);
                        nameFile = responsabilite;
                    }

                //l'ajout dans le dossier Files
                string name = Session["P_id"] + "_" + Session["S_id"] + "_" + typeDoc + fileExtension;

                string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(nameFile.FileName.Replace(nameFile.FileName, name)));

                            nameFile.SaveAs(path);

                    //l'ajout dans la base de données
                    Document doc = new Document();
                    doc.Dossier_Id_Dossier = dossier.Id_Dossier;
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

            return Redirect("/Account/UserPannel");

        }

        }

    }
