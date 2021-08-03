using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Artigos.Models
{
    public class ModelViewEscritorCadastro
    {
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100)]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MinLength(10)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Nível de Acesso")]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        [MaxLength(50)]
        public string NivelAcesso { get; set; }
        public List<SelectListItem> Items { get; set; }
    }
}