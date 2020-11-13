using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public struct CredenciaisDto
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
