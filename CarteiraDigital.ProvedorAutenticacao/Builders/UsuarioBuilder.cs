using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.Servicos;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Builders
{
    public class UsuarioBuilder : IUsuarioBuilder
    {
        private string nomeUsuario;
        private string nome;
        private string sobrenome;
        private string cpf;
        private DateTime dataNascimento;

        public IUsuarioBuilder ComNomeUsuario(string nomeUsuario)
        {
            this.nomeUsuario = nomeUsuario;
            return this;
        }

        public IUsuarioBuilder ComNome(string nome)
        {
            this.nome = nome;
            return this;
        }

        public IUsuarioBuilder ComSobrenome(string sobrenome)
        {
            this.sobrenome = sobrenome;
            return this;
        }

        public IUsuarioBuilder ComCpf(string cpf)
        {
            this.cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            return this;
        }

        public IUsuarioBuilder NascidoEm(DateTime dataNascimento)
        {
            this.dataNascimento = dataNascimento;
            return this;
        }

        public Usuario Gerar()
        {
            return new Usuario(nomeUsuario, nome, sobrenome, cpf, dataNascimento);
        }
    }
}
