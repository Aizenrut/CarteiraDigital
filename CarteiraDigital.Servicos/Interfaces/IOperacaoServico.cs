using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public interface IOperacaoServico
    {
        void MarcarPendente(Operacao operacao);
        void MarcarEfetivada(Operacao operacao);
        void MarcarComErro(Operacao operacao);
        void AlterarStatusTemplate(Operacao operacao, StatusOperacao novoStatus);
        void Creditar(Conta conta, decimal valor);
        void Debitar(Conta conta, decimal valor);
        void AlterarValoresTemplate(Conta conta, decimal valor, Action<Conta> acaoPrincipal);
        void ValidarValor(decimal valor);
        void ValidarSaldo(Conta conta, decimal valor);
        void ValidarArgumentoTemplate(bool condicao, string mensagem);
    }
}
