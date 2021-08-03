using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class ModelViewEscritorAcesso
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Nível de Acesso")]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        [MaxLength(50)]
        public string NivelAcesso { get; set; }
    }
}