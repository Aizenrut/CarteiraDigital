using CarteiraDigital.ProvedorAutenticacao.Models;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Builders
{
    public interface IUsuarioBuilder
    {
        IUsuarioBuilder ComNomeUsuario(string nomeUsuario);
        IUsuarioBuilder ComNome(string nome);
        IUsuarioBuilder ComSobrenome(string sobrenome);
        IUsuarioBuilder ComCpf(string cpf);
        IUsuarioBuilder NascidoEm(DateTime dataNascimento);
        Usuario Gerar();
    }
}
