using System.Data.Entity;

namespace Artigos.Models
{
    public class Context:DbContext
    {
        public Context() : base("Artigos") { }

        public DbSet<Escritor> Escritores { get; set; }
        public DbSet<Artigo> Artigos { get; set; }
        public DbSet<ArtigoCategoria> ArtigoCategorias { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Paragrafo> Paragrafos { get; set; }
        public DbSet<Imagem> Imagems { get; set; }
    }
}