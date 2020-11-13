using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public struct AlteracaoSenhaDto
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required]
        public string NovaSenha { get; set; }
    }
}
