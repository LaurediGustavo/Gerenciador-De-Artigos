namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ativa2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artigoes", "Ativo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artigoes", "Ativo");
        }
    }
}
