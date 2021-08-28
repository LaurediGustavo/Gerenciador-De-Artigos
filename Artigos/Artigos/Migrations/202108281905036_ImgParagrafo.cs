namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImgParagrafo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Imagems", "ParagrafoId");
            AddForeignKey("dbo.Imagems", "ParagrafoId", "dbo.Paragrafoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Imagems", "ParagrafoId", "dbo.Paragrafoes");
            DropIndex("dbo.Imagems", new[] { "ParagrafoId" });
        }
    }
}
