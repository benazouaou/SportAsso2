namespace SportAsso.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SportAsso.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SportAsso.Models.Context_db>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Context_db context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            Personne p = new Personne
            {
                Nom = "Truc",
                Prenom = "Michel",
                Date_Naissance = new System.DateTime(1980, 12, 12),
                E_mail = "michel.truc@sport.com",
                Num_Telephone = "0610101010",
                Sexe = "male",
                Mot_de_Passe = "michel",
                Confirm_Mot_Passe = "michel"
            };
            context.Personne.AddOrUpdate(p);

            Personne p2 = new Personne
            {
                Nom = "Dupont",
                Prenom = "Toto",
                Date_Naissance = new System.DateTime(1980, 11, 19),
                E_mail = "dupont.toto@sport.com",
                Num_Telephone = "0620101010",
                Sexe = "male",
                Mot_de_Passe = "toto",
                Confirm_Mot_Passe = "toto"
            };
            context.Personne.AddOrUpdate(p2);


            Personne p3 = new Personne
            {
                Nom = "Smith",
                Prenom = "Admin",
                Date_Naissance = new System.DateTime(1985, 10, 12),
                E_mail = "smith.admin@sport.com",
                Num_Telephone = "0610102010",
                Sexe = "male",
                Mot_de_Passe = "admin",
                Confirm_Mot_Passe = "admin"
            };
            context.Personne.AddOrUpdate(p3);
            context.SaveChanges();

            Personne encadrant = context.Personne
                .Where(enca => enca.E_mail == p.E_mail)
                .FirstOrDefault();

            Role admin = new Role
            {
                Nom_Role = "Admin"
            };
          

            Role adh = new Role
            {
                Nom_Role = "Adhérent"
            };
      

            Role enc = new Role
            {
                Nom_Role = "Encadrant"
            };

            context.Role.AddOrUpdate(admin);
            context.Role.AddOrUpdate(adh);
            context.Role.AddOrUpdate(enc);
            context.SaveChanges();

            p.Role.Add(enc);
            p.Role.Add(adh);
            p2.Role.Add(adh);
            p3.Role.Add(admin);
            context.SaveChanges();

            Lieu lieu = new Lieu
            {
                Nom = "Gymnase A",
                Adresse = "1 rue du sport"
            };


            Lieu lieu2 = new Lieu
            {
                Adresse = "2 rue du sport",
                Nom = "Dojo zen"
            };

            context.Lieu.Add(lieu);
            context.Lieu.Add(lieu2);
            context.SaveChanges();


            Lieu lieu1 = context.Lieu
                .Where(l => l.Nom == lieu.Nom)
                .FirstOrDefault();

            Lieu lieu2bis = context.Lieu
                .Where(l => l.Nom == lieu2.Nom)
                .FirstOrDefault();

            Discipline d = new Discipline
            {
                Nom_Discipline = "Basketball"
            };

            context.Discipline.AddOrUpdate(d);
            context.SaveChanges();

            Discipline basket = context.Discipline
                .Where(disci => disci.Nom_Discipline == d.Nom_Discipline)
                .FirstOrDefault();

            Section s1 = new Section
            {
                Nom = "Enfant",
                Prix = 80,
                Discipline_Id_Discipline = basket.Id_Discipline
            };
            context.Section.AddOrUpdate(s1);
            context.SaveChanges();

            Section section1 = context.Section
                .Where(s => s.Nom == s1.Nom)
                .FirstOrDefault();

            Section s2 = new Section
            {
                Nom = "Adulte",
                Prix = 100,
                Discipline_Id_Discipline = basket.Id_Discipline
            };
            context.Section.AddOrUpdate(s2);
            context.SaveChanges();

            Section section2 = context.Section
                .Where(s => s.Nom == s2.Nom)
                .FirstOrDefault();

            Creneau c1 = new Creneau
            {
                Nombre_Places_Dispo = 20,
                Nombre_Places_Max = 20,
                Section_Id_Section = section1.Id_Section,
                Lieu_Id_Lieu = lieu1.Id_Lieu,
                Encadrant = encadrant.Id_Personne,
                Jour = "Lundi",
                Heure = new System.TimeSpan(12)
            };
            context.Creneau.AddOrUpdate(c1);

            Creneau c2 = new Creneau
            {
                Nombre_Places_Dispo = 20,
                Nombre_Places_Max = 20,
                Section_Id_Section = section1.Id_Section,
                Lieu_Id_Lieu = lieu1.Id_Lieu,
                Encadrant = encadrant.Id_Personne,
                Jour = "Mardi",
                Heure = new System.TimeSpan(12)
            };
            context.Creneau.AddOrUpdate(c2);

            Creneau c3 = new Creneau
            {
                Nombre_Places_Dispo = 20,
                Nombre_Places_Max = 20,
                Section_Id_Section = section2.Id_Section,
                Lieu_Id_Lieu = lieu1.Id_Lieu,
                Encadrant = encadrant.Id_Personne,
                Jour = "Mercredi",
                Heure = new System.TimeSpan(12)
            };
            context.Creneau.AddOrUpdate(c3);
            context.SaveChanges();

            Discipline d2 = new Discipline
            {
                Nom_Discipline = "Gymnastique"
            };
            context.Discipline.AddOrUpdate(d2);
            context.SaveChanges();

            Discipline gym = context.Discipline
                .Where(disci => disci.Nom_Discipline == d2.Nom_Discipline)
                .FirstOrDefault();

            Section s3 = new Section
            {
                Nom = "Adulte",
                Prix = 100,
                Discipline_Id_Discipline = gym.Id_Discipline
            };
            context.Section.AddOrUpdate(s3);
            context.SaveChanges();

            Section section3 = context.Section
                .Where(se => se.Nom == s3.Nom && se.Discipline_Id_Discipline == s3.Discipline_Id_Discipline)
                .FirstOrDefault();

            Section s4 = new Section
            {
                Nom = "Compétititon",
                Prix = 120,
                Discipline_Id_Discipline = gym.Id_Discipline
            };
            context.Section.AddOrUpdate(s4);
            context.SaveChanges();

            Section section4 = context.Section
                .Where(se => se.Nom == s4.Nom && se.Discipline_Id_Discipline == s4.Discipline_Id_Discipline)
                .FirstOrDefault();


            Creneau c4 = new Creneau
            {
                Nombre_Places_Dispo = 20,
                Nombre_Places_Max = 20,
                Section_Id_Section = section3.Id_Section,
                Lieu_Id_Lieu = lieu2bis.Id_Lieu,
                Encadrant = encadrant.Id_Personne,
                Jour = "Jeudi",
                Heure = new System.TimeSpan(12)
            };
            context.Creneau.AddOrUpdate(c4);

            Creneau c5 = new Creneau
            {
                Nombre_Places_Dispo = 20,
                Nombre_Places_Max = 20,
                Section_Id_Section = section4.Id_Section,
                Lieu_Id_Lieu = lieu2bis.Id_Lieu,
                Encadrant = encadrant.Id_Personne,
                Jour = "Vendredi",
                Heure = new System.TimeSpan(12)
            };
            context.Creneau.AddOrUpdate(c5);
            context.SaveChanges();
        }
    }
}
