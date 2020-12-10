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
            //redirige vers la page principale de l'espace adherent
            return RedirectToRoute("/Account/UserPannel");
        }

        //methode en get
        [HttpGet]
        public ActionResult ModifierMdp()
        {
            //retourne la vue de modification du mot de passe
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult ModifierMdp(FormCollection id)
        {
            //recupere les donnees envoyees par le formulaire
            var ancien = Request.Form["ancien"];
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            
            //recupere dans la session l'id de l'adherent
            int Id = (int)Session["P_id"];

            //initalise error a false
            var error = false;

            using (var context = new Context_db())
            {
                //recupere l'adherent a partir de son id
                Personne mdp = context.Personne
                      .Where(p => p.Id_Personne == Id)
                      .FirstOrDefault();
                //envoie le mot de passe de cette adherent
                ViewBag.mdp = mdp.Mot_de_Passe;

                //si l'ancien mot de passe fourni par l'adherent est different de celui stocke en base de donnees
                if (ancien != ViewBag.mdp)
                {
                    //passe error a true
                    error = true;
                    //envoi a la vue un message d'erreur
                    ViewBag.Ancien = "Mot de passe erroné";
                }
                //si le nouveau mot de passe fourni est different de celui fourni pour le confirmer
                if (nouveau != nouveauConf || nouveau == "")
                {
                    //passe error a true
                    error = true;
                    //envoi un message d'erreur a la vue
                    ViewBag.Nouveau = "Les deux mots de passe sont différents ou vides";
                }
                //si le nouveau mot de passe fourni est inferieur a 4 caracteres (minimum autorise dans la base de donnees)
                if (nouveau.Length < 4)
                {
                    //passe error a true
                    error = true;
                    //envoi un message d'erreur a la vue
                    ViewBag.Nouveau = "Nouveau mot de passe trop court (au moins 4 caractères)";
                }
            }
            //s'il n'y a pas eu d'erreur pendant les verifications
            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        //recupere l'adherent
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        //met a jour son mot de passe
                        personne.Mot_de_Passe = (string)nouveau;
                        personne.Confirm_Mot_Passe = (string)nouveau;
                        //enregistre les modifications dans la base de donnees
                        context.SaveChanges();
                        //renvoi l'adherent a la page d'accueil de son espace personnel
                        return Redirect("/Account/UserPannel");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        //en cas de probleme affiche les messages d'erreur
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
        //si error vaut true ou qu'une erreur a eu lieu dans le try catch renvoi la page avec les messages d'erreur crees
        return View();
        }

        //methode en get
        [HttpGet]
        public ActionResult ModifierMail()
        {
            //renvoi la vue permettant a un adherent de modifier son email
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult ModifierMail(FormCollection id)
        {
            //recupere les donnees recues via le formulaire
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            
            //recupere dans la session l'id de l'adherent
            int Id = (int)Session["P_id"];

            //Initialise error a false
            var error = false;

            //si la nouvelle adresse email et la confirmation sont diffenrentes ou si l'adresse mail est vide
            if (nouveau != nouveauConf || nouveau == "")
            {
                //passe error a true
                error = true;
                //renvoi un message d'erreur a la vue
                ViewBag.Nouveau = "Les deux mots de passe sont différents ou vides";
            }

            //s'il n'y a pas eu  d'erreur
            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        //recupere l'adherent
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        //met a jour son champ E_mail
                        personne.E_mail = (string)nouveau;
                        //enregistre les modififcations
                        context.SaveChanges();
                        //redirige l'adherent vers la page d'accueil de son espace personnel
                        return Redirect("/Account/UserPannel");
                    }
                    //affiche les erreurs s'il y a eu un probleme dans le try catch
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
            //si error vaut true ou qu'une erreur a eu lieu dans le try catch renvoi la page avec les messages d'erreur crees
            return View();
        }

        //methode en get
        [HttpGet]
        public ActionResult ModifierTel()
        {
            //rectourne la vue permettant de modifier son numero de telephone
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult ModifierTel(FormCollection id)
        {
            //recupere les donnees recues via le formulaire
            var nouveau = Request.Form["nouveau"];
            var nouveauConf = Request.Form["nouveauConf"];
            
            //recupere l'id de l'adherent
            int Id = (int)Session["P_id"];

            //initialise erro a false
            var error = false;

            //si le nouveau numero est different de la confirmation ou vide
            if (nouveau != nouveauConf || nouveau == "")
            {
                //passe error a true
                error = true;
                //retourne un message d'erreur a la vue
                ViewBag.Nouveau = "Les deux numéros de téléphone sont différents ou vides";
            }

            //s'il n'y a pas eu d'erreur
            if (error == false)
            {
                using (var context = new Context_db())
                {
                    try
                    {
                        //recupere l'adherent
                        Personne personne = context.Personne
                            .Where(pe => pe.Id_Personne == Id)
                            .FirstOrDefault();
                        //met a jour son champ Num_Telephone
                        personne.Num_Telephone = (string)nouveau;
                        //enregistre les modifications
                        context.SaveChanges();
                        //renvoie l'adherent a la page d'accueil de son espace personnel
                        return Redirect("/Account/UserPannel");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        //afiiche les erreurs en cas de probleme dans le try catch
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
            //si error vaut true ou qu'une erreur a eu lieu dans le try catch renvoi la page avec les messages d'erreur crees
            return View();
        }

        //methode en get
        [HttpGet]
        public ActionResult ModifierCreneau(int id)
        {
            using (var context = new Context_db())
            {
                //recupere le creneau dont l'id est passe en parametre
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == id)
                    .FirstOrDefault();
                //le passe a la vue
                ViewBag.Creneau = creneau;

                //recupere la liste de tous les creneaux appartenant a la meme section que celui recupere
                List<Creneau> creneaux = context.Creneau
                   .Where(c => c.Section_Id_Section == creneau.Section_Id_Section)
                   .ToList();
                //passe la liste a la vue
                ViewBag.Creneaux = creneaux;
            }
            //retourne la vue qui permet de choisir un nouveau creneau pour sa pre-inscription
            return View();
        }

        //methode en post
        [HttpPost]
        public ActionResult ModifierCreneau(FormCollection id)
        {
            //recupere les donnees recues via le formulaire
            var ancien = int.Parse(Request.Form["ancien"]);
            var nouveau = int.Parse(Request.Form["nouveau"]);

            //recupere l'id de l'adherent dans session
            int Id = (int)Session["P_id"];

            using (var context = new Context_db())
            {
                try
                {
                    //recupere l'ancien creneau choisi par l'adherent
                    Creneau ancienC = context.Creneau
                        .Where(c => c.Id_Creneau == ancien)
                        .FirstOrDefault();

                    //recupere le nouveau creneau choisi par l'adherent
                    Creneau nouveauC = context.Creneau
                        .Where(c => c.Id_Creneau == nouveau)
                        .FirstOrDefault();

                    //met a jour le nombre de places disponibles dans chacun d'entre eux
                    nouveauC.Nombre_Places_Dispo = nouveauC.Nombre_Places_Dispo - 1;
                    ancienC.Nombre_Places_Dispo = ancienC.Nombre_Places_Dispo + 1;
                    
                    //recupere l'adherent
                    Personne adherent = context.Personne
                        .Where(p => p.Id_Personne == Id)
                        .FirstOrDefault();

                    //met a jour la table inscrits de la base de donnees
                    adherent.Creneau1.Add(nouveauC);
                    adherent.Creneau1.Remove(ancienC);
                    //enregistre les modifications
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    //affiche les erreurs en cas de probleme dans le try catch
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            //renvoi la page d'accueil de l'espace personnel de l'utilisateur
            return Redirect("/Account/UserPannel");
        }

        //methode en post
        [HttpPost]
        public ActionResult SupprimerDossier(FormCollection id)
        {
            //recupere les donnees recues via le formulaire
            var dossierId = int.Parse(Request.Form["dossier"]);
            var creneauId = int.Parse(Request.Form["creneau"]);
            
            using (var context = new Context_db())
            {
                //recupere le dossier a supprimer
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == dossierId)
                    .FirstOrDefault();
                //recupere le creneau reserve dans ce dossier
                Creneau creneau = context.Creneau
                    .Where(c => c.Id_Creneau == creneauId)
                    .FirstOrDefault();
                //recupere l'adherent
                Personne adherent = context.Personne
                    .Where(p => p.Id_Personne == dossier.Personne_Id_Personne)
                    .FirstOrDefault();
                //recupere tous les documents lies au dossier
                List<Document> docs = context.Document
                    .Where(d => d.Dossier_Id_Dossier == dossier.Id_Dossier)
                    .ToList();
                try
                {
                    //pour chaque document du dossier
                    foreach (var doc in docs as List<Document>)
                    {
                        //recupere le chemin d'acces au fichier 
                        string path = doc.Path;
                        //supprime le fichier stocke
                        System.IO.File.Delete(path);
                        //supprime la ligne du fichier dans la base de donnees
                        context.Document.Remove(doc);
                    }

                    //supprime le dossier de la base de donnees
                    context.Dossier.Remove(dossier);

                    //met a jour la table inscrits de la base de donnees
                    adherent.Creneau1.Remove(creneau);

                    //met a jour le nombre de places disponible pour le creneau
                    creneau.Nombre_Places_Dispo = creneau.Nombre_Places_Dispo + 1;

                    //enregistre les modifications
                    context.SaveChanges();

                }
                //affiche les erreurs en cas de probleme dans le try catch
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
            //renvoi la page avec les messages d'erreur crees
            return Redirect("/Account/UserPannel");
        }

        //methode en post
        [HttpPost]
        [ValidateAntiForgeryToken] // permet d'empêcher les attaques de falsification de requêtes intersites
        public ActionResult ModificationDoc()
        {
            //recupere tous les documents pouvant être envoyes (un seul possible a chaque fois)
            HttpPostedFileBase responsabilite = Request.Files["responsabilite"];
            HttpPostedFileBase renseignement = Request.Files["renseignement"];
            HttpPostedFileBase medical = Request.Files["medical"];

            using (var context = new Context_db())
            {
                //récupére l'id du dossier et l'id de l'adhernet dans session
                int id_dossier = (int) Session["id_dossier"];
                int Id = (int) Session["P_id"];

                //recupere le dossier
                Dossier dossier = context.Dossier
                    .Where(d => d.Id_Dossier == id_dossier)
                    .FirstOrDefault();
                //passe l'id du dossier a la vue
                ViewBag.DossierId = dossier.Id_Dossier;

                try
                {
                string typeDoc;
                string fileExtension;
                HttpPostedFileBase nameFile;
                    if (renseignement != null)  //si le document transmis estla fiche de renseignement
                    {
                        //iniitalise le type de document, l'extension du document, et stocke le document dans nameFile
                        typeDoc = "Fiche de renseignement";
                        fileExtension = Path.GetExtension(renseignement.FileName);
                        nameFile = renseignement;
                    }
                    else if (medical != null)   //si le document transmis est l'attestation medicale
                    {
                        //iniitalise le type de document, l'extension du document, et stocke le document dans nameFile
                        typeDoc = "Attestation medicale";
                        fileExtension = Path.GetExtension(medical.FileName);
                        nameFile = medical;
                    }
                    else  //sinon il s'agit alors de l'assurance de responsabilite civile
                    {
                        //iniitalise le type de document, l'extension du document, et stocke le document dans nameFile
                        typeDoc = "Assurance de responsabilité civile";
                        fileExtension = Path.GetExtension(responsabilite.FileName);
                        nameFile = responsabilite;
                    }

                //cree un nouveau nom pour le fichier qui contient le type du document l'id de l'adherent et la section a laquelle il s'inscrit 
                //comme il ne peut s'inscrire qu'a un seul crenenau dans une section cela garanti l'unicite des noms de fichier
                string name = Session["P_id"] + "_" + Session["S_id"] + "_" + typeDoc + fileExtension;
                   
                //l'ajoute dans le dossier Files en generant le chemin absolu et en remplaçant le nom du fichier par celui cree
                string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(nameFile.FileName.Replace(nameFile.FileName, name)));
                nameFile.SaveAs(path);

                //ajoute le document dans la base de donnees en creant et remplissant un nouveau document dans le modele
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
                    //affiche l'erreur en cas de probleme dans le try catch
                    ViewBag.message = "Erreur !" + ex;
                }
            }
            //redirige l'adherent vers la page de la preinscription ou il vient d'ajouter un document
            return Redirect("/Account/Preinscription/"+ViewBag.DossierId);
        }

        }
    }
