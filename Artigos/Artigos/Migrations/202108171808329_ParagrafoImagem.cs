namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParagrafoImagem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Imagems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParagrafoId = c.Int(nullable: false),
                        Img = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Paragrafoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArtigoId = c.Int(nullable: false),
                        Texto = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Paragrafoes");
            DropTable("dbo.Imagems");
        }
    }
}
