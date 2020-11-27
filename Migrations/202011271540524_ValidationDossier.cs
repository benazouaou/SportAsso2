namespace SportAsso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationDossier : DbMigration
    {
        public override void Up()
        {
            //AddColumn("Dossier", "Est_Valide", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("Dossier", "Est_Valide");
        }
    }
}
