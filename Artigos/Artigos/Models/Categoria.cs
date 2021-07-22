using System.ComponentModel.DataAnnotations;

namespace Artigos.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(50, ErrorMessage = "Esse campo não pode ser maior que 50!")]
        public string NomeCategoria { get; set; }
        [MaxLength(1, ErrorMessage = "Esse campo não pode ser maior que 1!")]
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public string Ativa { get; set; }
    }
}