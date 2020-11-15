using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public interface IOperacaoServico
    {
        void Creditar(Conta conta, decimal valor);
        void Debitar(Conta conta, decimal valor);
        void ValidarValor(decimal valor);
        void ValidarSaldo(Conta conta, decimal valor);
        void RealizarOperacaoTemplate(Conta conta, decimal valor, Action<Conta> acaoPrincipal);
        void RealizarValidacaoArgumentoTemplate(bool condicao, string mensagem);
    }
}
