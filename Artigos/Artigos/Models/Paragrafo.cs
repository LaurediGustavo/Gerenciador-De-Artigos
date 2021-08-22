using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class Paragrafo
    {
        [Key]
        public int Id { get; set; }
        public int ArtigoId { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public string Texto { get; set; }
        public virtual IEnumerable<Imagem> Imagems { get; set; }
    }
}