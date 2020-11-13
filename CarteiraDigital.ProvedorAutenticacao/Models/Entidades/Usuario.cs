using Microsoft.AspNetCore.Identity;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }

        public Usuario()
        {
        }

        public Usuario(string nomeUsuario) : base(nomeUsuario)
        {
            Ativo = true;
        }
    }
}
