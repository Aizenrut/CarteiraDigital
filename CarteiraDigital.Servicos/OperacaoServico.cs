using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class OperacaoServico : IOperacaoServico
    {
        public void Creditar(Conta conta, decimal valor)
        {
            Action<Conta> acaoPrincipal = conta =>
            {
                conta.Saldo += valor;
            };

            RealizarOperacaoTemplate(conta, valor, acaoPrincipal);
        }

        public void Debitar(Conta conta, decimal valor)
        {
            Action<Conta> acaoPrincipal = conta =>
            {
                ValidarSaldo(conta, valor);
                conta.Saldo -= valor;
            };

            RealizarOperacaoTemplate(conta, valor, acaoPrincipal);
        }

        public void ValidarValor(decimal valor)
        {
            RealizarValidacaoArgumentoTemplate(valor <= 0, "O valor da operação deve ser superior a zero!");
        }

        public void ValidarSaldo(Conta conta, decimal valor)
        {
            RealizarValidacaoArgumentoTemplate(conta.Saldo < valor, "O saldo da conta é insuficiente para realizar a operação!");
        }

        public void RealizarOperacaoTemplate(Conta conta, decimal valor, Action<Conta> acaoPrincipal)
        {
            ValidarValor(valor);
            acaoPrincipal(conta);
        }

        public void RealizarValidacaoArgumentoTemplate(bool condicao, string mensagem)
        {
            if (condicao)
                throw new ArgumentException(mensagem);
        }
    }
}
