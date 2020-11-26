using System;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web;
using SportAsso.Models;

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
                        personne.Mot_de_Passe = (string) nouveau;
                        personne.Confirm_Mot_Passe = (string) nouveau;
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
                        personne.E_mail = (string) nouveau;
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
    }
}