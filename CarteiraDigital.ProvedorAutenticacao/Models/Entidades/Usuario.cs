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
        public bool Ativo { get; set; }

        public Usuario()
        {
        }

        public Usuario(string nomeUsuario, string nome, string sobrenome, string cpf, DateTime dataNascimento) : base(nomeUsuario)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Ativo = true;
        }
    }
}
