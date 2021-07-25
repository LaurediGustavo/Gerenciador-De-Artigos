namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ativa1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categorias", "Ativa", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categorias", "Ativa", c => c.String());
        }
    }
}
