using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewArtigoEdicao
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Digite no máximo 100 caracteres!")]
        public string Titulo { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Capa")]
        public HttpPostedFileBase Image { get; set; }
        public Byte[] Capa { get; set; }
        public bool Ativa { get; set; }
        public List<Categoria> Categorias { get; set; }
    }

    public class ModelViewArtigoEdicaoCategoria
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public List<Categoria> Categorias { get; set; }
    }
}