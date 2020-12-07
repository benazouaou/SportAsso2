namespace SportAsso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class docPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Document", "Path", c => c.String(nullable: false));
            DropColumn("dbo.Document", "Fichier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Document", "Fichier", c => c.Binary(nullable: false));
            DropColumn("dbo.Document", "Path");
        }
    }
}
