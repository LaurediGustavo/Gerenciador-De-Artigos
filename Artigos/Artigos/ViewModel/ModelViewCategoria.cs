using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewCategoria
    {
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(50, ErrorMessage = "Esse campo não pode ser maior que 50!")]
        [Display(Name = "Nome Categoria")]
        public string NomeCategoria { get; set; }
        public Boolean Ativa { get; set; }
    }
}