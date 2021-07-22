using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class Artigo
    {
        [Key]
        public int Id { get; set; }
        public int EscritorId { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Digite no máximo 100 caracteres!")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public byte[] Capa { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public char Ativo { get; set; }
    }
}