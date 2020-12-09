using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportAsso
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Discipline",
                url: "Discipline/{action}/{id}/{id2}/{id3}",
                //id = discipline id2=section id3=creneau
                defaults: new { controller = "Discipline", action = "Index", id = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional }
);

            routes.MapRoute(
                 name: "inscrip",
                 url: "InscriptionCreneau/{action}/{id}/{id2}",
                 //id = discipline id2=section 
                 defaults: new { controller = "InscriptionCreneau", action = "Index" }
);
            

            routes.MapRoute(
               name: "Account",
               url: "Account/{action}/{id}",
               //id = numéro du dossier en get 
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
);

            routes.MapRoute(
              name: "GestionA",
              url: "GestionA/{action}/{id}",
              defaults: new { controller = "GestionAdherent", action = "Index", id = UrlParameter.Optional }
 );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
 );
        }
    }
}
