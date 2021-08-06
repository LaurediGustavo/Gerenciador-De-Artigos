using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewArtigo
    {
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Digite no máximo 100 caracteres!")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Capa { get; set; }

        public List<Categoria> Categorias { get; set; }

        public Boolean Ativo { get; set; }
    }
}