using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artigos.Models
{
    public class ArtigoCategoria
    {
        [Key]
        public int Id { get; set; }
        public int ArtigoId { get; set; }
        public int CategoriaId { get; set; }

        public virtual ICollection<Categoria> Categorias { get; set; }
        public virtual ICollection<Artigo> Artigos { get; set; }
    }
}