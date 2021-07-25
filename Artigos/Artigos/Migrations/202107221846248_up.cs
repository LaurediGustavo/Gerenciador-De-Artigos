namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class up : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categorias", "Ativa", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categorias", "Ativa", c => c.String(nullable: false, maxLength: 1));
        }
    }
}
