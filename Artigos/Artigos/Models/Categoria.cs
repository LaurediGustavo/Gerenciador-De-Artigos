using System.ComponentModel.DataAnnotations;

namespace Artigos.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(50, ErrorMessage = "Esse campo não pode ser maior que 50!")]
        [Display(Name = "Nome Categoria")]
        public string NomeCategoria { get; set; }
        public int Ativa { get; set; }
    }
}