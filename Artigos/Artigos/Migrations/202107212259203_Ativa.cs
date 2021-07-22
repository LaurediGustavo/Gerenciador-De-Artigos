namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ativa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categorias", "Ativa", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categorias", "Ativa");
        }
    }
}
