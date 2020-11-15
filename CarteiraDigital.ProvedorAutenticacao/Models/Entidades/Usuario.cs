using CarteiraDigital.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public partial class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Conta Conta { get; set; }
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
