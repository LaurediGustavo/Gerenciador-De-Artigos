using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artigos.Models
{
    public class Escritor
    {
        [Key]
        public int Id { get; set; }

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
        [MinLength(8, ErrorMessage = "A senha não pode ser menor que 8!")]
        [MaxLength(12, ErrorMessage = "A senha não pode ser maior que 12!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MinLength(10)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Nível de Acesso")]
        [Required(ErrorMessage = "Esse campo é obrigatório")]
        [MaxLength(50)]
        public string NivelAcesso { get; set; }

        public virtual ICollection<Artigo> Artigos { get; set; }
    }
}