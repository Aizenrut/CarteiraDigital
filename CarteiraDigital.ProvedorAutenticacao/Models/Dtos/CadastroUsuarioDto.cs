using System;
using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public struct CadastroUsuarioDto
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Sobrenome { get; set; }
        
        [Required]
        public string Cpf { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
