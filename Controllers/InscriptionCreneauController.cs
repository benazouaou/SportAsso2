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
            //recupere le creneau dont l'id est passe en parametre
            Creneau creneau = context.Creneau
                .Where(x => x.Id_Creneau == id)
                .FirstOrDefault();
            //passe a la vue le creneau et son id
            ViewBag.Creneau = creneau;
            ViewBag.idCreneau = creneau.Id_Creneau;
            //enregistre dans la session l'id du creneau
            Session["Creneau"] = creneau.Id_Creneau;

            //recupere la section a laquelle appartient le creneau
            Section s = context.Section
                .Where(x => x.Id_Section == creneau.Section_Id_Section)
                .FirstOrDefault();
            //passe a la vue les informations sur la section
            ViewBag.SectionId = s.Id_Section;
            ViewBag.SectionCreneau = s.Nom;
            ViewBag.PrixSection = s.Prix;
            //enregistre dans la session l'id de la section
            Session["S_id"]= s.Id_Section;

            //recupere le lieu ou se passe le cours du creneau
            Lieu l = context.Lieu
                .Where(x => x.Id_Lieu == creneau.Lieu_Id_Lieu)
                .FirstOrDefault();
            //passe a la vue les informations sur le lieu
            ViewBag.LieuCreneau = l.Nom;
            ViewBag.AdresseCreneau = l.Adresse;

            //recupere les informations sur l'encadrant du creneau
            Personne p = context.Personne
                .Where(x => x.Id_Personne == creneau.Encadrant)
                .FirstOrDefault();
            //passe a la vue cet encadrant
            ViewBag.Encadrant = p;

            //recupere la discipline du creneau
            Discipline d = context.Discipline
                .Where(x => x.Id_Discipline == s.Discipline_Id_Discipline)
                .FirstOrDefault();
            //passe le nom de la discipline a la vue
            ViewBag.CreneauDiscipline = d.Nom_Discipline;

           //retourne la vue
            return View();
        }

        // methode appelee en POST
        [HttpPost]
        // permet d'empêcher les attaques de falsification de requêtes intersites
        [ValidateAntiForgeryToken] 
        public ActionResult Confirmation(HttpPostedFileBase[] nameFile)
        {
            //cree un nouveau dossier et le rempli avec les informations de session
             Dossier d = new Dossier();
             d.Section_Id_Section = (int)Session["S_id"];
             d.Personne_Id_Personne = (int)Session["P_id"];
             d.Paiement = 1;
            //ajoute le dossier dans la base de donnees en enregistre les modifications
             context.Dossier.Add(d);
             context.SaveChanges();

            //recupere la section du dossier cree
            Section section = context.Section
                .Where(s => s.Id_Section == d.Section_Id_Section)
                .FirstOrDefault();
            //envoie la section a la page de confirmation
            ViewBag.Section = section;

            //recupere la discipline de la section
            Discipline discipline = context.Discipline
                .Where(dos => dos.Id_Discipline == section.Discipline_Id_Discipline)
                .FirstOrDefault();
            //l'envoie a la apge de confirmation
            ViewBag.Discipline = discipline;

            //parcours la liste de documents reçue en parametre
            for (int i = 0; i < 3; i++)
            {
                //si le document n'est pas null
                if (nameFile[i] != null)
                    try
                    {
                        string typeDoc;
                        //recupere l'extension du document
                        string fileExtension = System.IO.Path.GetExtension(nameFile[i].FileName);
                        //initialise type doc en fonction de l'ordre du document dans la liste
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
                        //cree un nouveau nom pour le fichier qui contient le type du document l'id de l'adherent et la section a laquelle il s'inscrit 
                        //comme il ne peut s'inscrire qu'a un seul crenenau dans une section cela garanti l'unicite des noms de fichier
                        string name = Session["P_id"] + "_" + Session["S_id"] + "_" + typeDoc + fileExtension;
                        
                        //l'ajoute dans le dossier Files en generant le chemin absolu et en remplaçant le nom du fichier par celui cree
                        string path = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(nameFile[i].FileName.Replace(nameFile[i].FileName, name)));
                        nameFile[i].SaveAs(path);

                        //ajoute le document dans la base de donnees en creant et remplissant un nouveau document dans le modele
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
            //recupere les id de la personne et du creneau
            int idP = (int)Session["P_id"];
            int ids = (int)Session["Creneau"];
            // met a jour la table inscrit
            Personne p = context.Personne.Where(x => x.Id_Personne == idP).FirstOrDefault();    
            Creneau cr = context.Creneau.Where(x => x.Id_Creneau == ids).FirstOrDefault();
            p.Creneau1.Add(cr);
            //sauve les modififcations
            context.SaveChanges();

            //decremente le nombre de places disponible dans le creneau
           cr.Nombre_Places_Dispo -= 1;
            context.SaveChanges();

            //retourne la vue de confirmation
            return View();

        }

        public FileResult Telecharger(string fileName)
        {
            //recupere le nom de fichier passe ne parametre
            fileName = "Fiche de renseignement.pdf";
            //genenre le chemin absolu pour recuperer ce fichier dans le dossier Files
            string path = Path.Combine(Server.MapPath("~/Files"), fileName);
            //recupere le fichier sous forme de tableau de byte
            byte[] fileByte = System.IO.File.ReadAllBytes(path);
            //retourne le fichier recupere
            return File(fileByte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}

