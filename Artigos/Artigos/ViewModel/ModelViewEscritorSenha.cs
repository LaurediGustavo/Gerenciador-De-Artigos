using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Artigos.Models
{
    public class EscritorSenha
    {
        [Required]
        public string SenhaAntiga { get; set; }
        [Required]
        public string SenhaNova { get; set; }
        [Required]
        [MinLength(8)]
        public string SenhaConfirma { get; set; }
    }
}