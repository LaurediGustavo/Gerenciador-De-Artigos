namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class texto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Escritors", "NivelAcesso", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Escritors", "NivelAcesso");
        }
    }
}
