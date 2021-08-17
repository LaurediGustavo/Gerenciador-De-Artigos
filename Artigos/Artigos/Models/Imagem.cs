using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class Imagem
    {
        [Key]
        public int Id { get; set; }
        public int ParagrafoId { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public byte[] Img { get; set; }
    }
}