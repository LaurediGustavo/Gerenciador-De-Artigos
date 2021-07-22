namespace Artigos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArtigoCategorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArtigoId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Artigoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EscritorId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Capa = c.Binary(nullable: false),
                        ArtigoCategoria_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArtigoCategorias", t => t.ArtigoCategoria_Id)
                .ForeignKey("dbo.Escritors", t => t.EscritorId, cascadeDelete: true)
                .Index(t => t.EscritorId)
                .Index(t => t.ArtigoCategoria_Id);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NomeCategoria = c.String(nullable: false, maxLength: 50),
                        ArtigoCategoria_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArtigoCategorias", t => t.ArtigoCategoria_Id)
                .Index(t => t.ArtigoCategoria_Id);
            
            CreateTable(
                "dbo.Escritors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Sobrenome = c.String(nullable: false, maxLength: 100),
                        Login = c.String(nullable: false, maxLength: 100),
                        Senha = c.String(nullable: false, maxLength: 12),
                        Email = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Artigoes", "EscritorId", "dbo.Escritors");
            DropForeignKey("dbo.Categorias", "ArtigoCategoria_Id", "dbo.ArtigoCategorias");
            DropForeignKey("dbo.Artigoes", "ArtigoCategoria_Id", "dbo.ArtigoCategorias");
            DropIndex("dbo.Categorias", new[] { "ArtigoCategoria_Id" });
            DropIndex("dbo.Artigoes", new[] { "ArtigoCategoria_Id" });
            DropIndex("dbo.Artigoes", new[] { "EscritorId" });
            DropTable("dbo.Escritors");
            DropTable("dbo.Categorias");
            DropTable("dbo.Artigoes");
            DropTable("dbo.ArtigoCategorias");
        }
    }
}
