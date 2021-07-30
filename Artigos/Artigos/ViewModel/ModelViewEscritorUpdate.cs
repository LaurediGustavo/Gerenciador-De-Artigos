using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewEscritorUpdate
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
    }
}