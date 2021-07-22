using System.ComponentModel.DataAnnotations;

namespace Artigos.Models
{
    public class ModelViewEscritorLogin
    {
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [MinLength(8, ErrorMessage = "A senha não pode ser menor que 8!")]
        [MaxLength(12, ErrorMessage = "A senha não pode ser maior que 12!")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}