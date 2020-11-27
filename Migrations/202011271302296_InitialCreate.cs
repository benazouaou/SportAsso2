namespace SportAsso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            /**CreateTable(
                "dbo.Creneau",
                c => new
                    {
                        Id_Creneau = c.Int(nullable: false, identity: true),
                        Nombre_Places_Dispo = c.Int(nullable: false),
                        Nombre_Places_Max = c.Int(nullable: false),
                        Section_Id_Section = c.Int(nullable: false),
                        Lieu_Id_Lieu = c.Int(nullable: false),
                        Encadrant = c.Int(nullable: false),
                        Jour = c.String(nullable: false, maxLength: 50, unicode: false),
                        Heure = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id_Creneau)
                .ForeignKey("dbo.Lieu", t => t.Lieu_Id_Lieu)
                .ForeignKey("dbo.Personne", t => t.Encadrant)
                .ForeignKey("dbo.Section", t => t.Section_Id_Section)
                .Index(t => t.Section_Id_Section)
                .Index(t => t.Lieu_Id_Lieu)
                .Index(t => t.Encadrant);
            
            CreateTable(
                "dbo.Lieu",
                c => new
                    {
                        Id_Lieu = c.Int(nullable: false, identity: true),
                        Adresse = c.String(nullable: false, maxLength: 150, unicode: false),
                        Nom = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id_Lieu);
            
            CreateTable(
                "dbo.Personne",
                c => new
                    {
                        Id_Personne = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50, unicode: false),
                        Prenom = c.String(nullable: false, maxLength: 50, unicode: false),
                        Date_Naissance = c.DateTime(nullable: false, storeType: "date"),
                        E_mail = c.String(nullable: false, maxLength: 70, unicode: false),
                        Num_Telephone = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sexe = c.String(maxLength: 10, unicode: false),
                        Mot_de_Passe = c.String(nullable: false, maxLength: 50, unicode: false),
                        Confirm_Mot_Passe = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id_Personne);
            
            CreateTable(
                "dbo.Dossier",
                c => new
                    {
                        Id_Dossier = c.Int(nullable: false, identity: true),
                        Paiement = c.Byte(nullable: false),
                        Personne_Id_Personne = c.Int(nullable: false),
                        Section_Id_Section = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Dossier)
                .ForeignKey("dbo.Section", t => t.Section_Id_Section)
                .ForeignKey("dbo.Personne", t => t.Personne_Id_Personne)
                .Index(t => t.Personne_Id_Personne)
                .Index(t => t.Section_Id_Section);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        Id_Doc = c.Int(nullable: false, identity: true),
                        Type_Document = c.String(nullable: false, maxLength: 150, unicode: false),
                        Dossier_Id_Dossier = c.Int(nullable: false),
                        Fichier = c.Binary(nullable: false),
                        Est_Valide = c.Byte(),
                    })
                .PrimaryKey(t => t.Id_Doc)
                .ForeignKey("dbo.Dossier", t => t.Dossier_Id_Dossier)
                .Index(t => t.Dossier_Id_Dossier);
            
            CreateTable(
                "dbo.Section",
                c => new
                    {
                        Id_Section = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50, unicode: false),
                        Prix = c.Decimal(nullable: false, precision: 20, scale: 0),
                        Discipline_Id_Discipline = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Section)
                .ForeignKey("dbo.Discipline", t => t.Discipline_Id_Discipline)
                .Index(t => t.Discipline_Id_Discipline);
            
            CreateTable(
                "dbo.Discipline",
                c => new
                    {
                        Id_Discipline = c.Int(nullable: false, identity: true),
                        Nom_Discipline = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id_Discipline);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id_Role = c.Int(nullable: false, identity: true),
                        Nom_Role = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id_Role);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.Personne_has_Role",
                c => new
                    {
                        Personne_Id_Personne = c.Int(nullable: false),
                        Role_Id_Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Personne_Id_Personne, t.Role_Id_Role })
                .ForeignKey("dbo.Personne", t => t.Personne_Id_Personne, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.Role_Id_Role, cascadeDelete: true)
                .Index(t => t.Personne_Id_Personne)
                .Index(t => t.Role_Id_Role);
            
            CreateTable(
                "dbo.Inscrits",
                c => new
                    {
                        Creneau_Id_Creneau = c.Int(nullable: false),
                        Personne_Id_Personne = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Creneau_Id_Creneau, t.Personne_Id_Personne })
                .ForeignKey("dbo.Creneau", t => t.Creneau_Id_Creneau, cascadeDelete: true)
                .ForeignKey("dbo.Personne", t => t.Personne_Id_Personne, cascadeDelete: true)
                .Index(t => t.Creneau_Id_Creneau)
                .Index(t => t.Personne_Id_Personne);
            **/
            
        }
        
        public override void Down()
        {
            /**
            DropForeignKey("dbo.Inscrits", "Personne_Id_Personne", "dbo.Personne");
            DropForeignKey("dbo.Inscrits", "Creneau_Id_Creneau", "dbo.Creneau");
            DropForeignKey("dbo.Personne_has_Role", "Role_Id_Role", "dbo.Role");
            DropForeignKey("dbo.Personne_has_Role", "Personne_Id_Personne", "dbo.Personne");
            DropForeignKey("dbo.Dossier", "Personne_Id_Personne", "dbo.Personne");
            DropForeignKey("dbo.Dossier", "Section_Id_Section", "dbo.Section");
            DropForeignKey("dbo.Section", "Discipline_Id_Discipline", "dbo.Discipline");
            DropForeignKey("dbo.Creneau", "Section_Id_Section", "dbo.Section");
            DropForeignKey("dbo.Document", "Dossier_Id_Dossier", "dbo.Dossier");
            DropForeignKey("dbo.Creneau", "Encadrant", "dbo.Personne");
            DropForeignKey("dbo.Creneau", "Lieu_Id_Lieu", "dbo.Lieu");
            DropIndex("dbo.Inscrits", new[] { "Personne_Id_Personne" });
            DropIndex("dbo.Inscrits", new[] { "Creneau_Id_Creneau" });
            DropIndex("dbo.Personne_has_Role", new[] { "Role_Id_Role" });
            DropIndex("dbo.Personne_has_Role", new[] { "Personne_Id_Personne" });
            DropIndex("dbo.Section", new[] { "Discipline_Id_Discipline" });
            DropIndex("dbo.Document", new[] { "Dossier_Id_Dossier" });
            DropIndex("dbo.Dossier", new[] { "Section_Id_Section" });
            DropIndex("dbo.Dossier", new[] { "Personne_Id_Personne" });
            DropIndex("dbo.Creneau", new[] { "Encadrant" });
            DropIndex("dbo.Creneau", new[] { "Lieu_Id_Lieu" });
            DropIndex("dbo.Creneau", new[] { "Section_Id_Section" });
            DropTable("dbo.Inscrits");
            DropTable("dbo.Personne_has_Role");
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.Role");
            DropTable("dbo.Discipline");
            DropTable("dbo.Section");
            DropTable("dbo.Document");
            DropTable("dbo.Dossier");
            DropTable("dbo.Personne");
            DropTable("dbo.Lieu");
            DropTable("dbo.Creneau");
            **/
        }
    }
}
