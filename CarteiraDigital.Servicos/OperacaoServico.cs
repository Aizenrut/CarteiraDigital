using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class OperacaoServico : IOperacaoServico
    {
        public void MarcarPendente(Operacao operacao)
        {
            AlterarStatusTemplate(operacao, StatusOperacao.Pendente);
        }

        public void MarcarEfetivada(Operacao operacao)
        {
            AlterarStatusTemplate(operacao, StatusOperacao.Efetivada);
        }

        public void MarcarComErro(Operacao operacao)
        {
            AlterarStatusTemplate(operacao, StatusOperacao.ComErro);
        }

        public void AlterarStatusTemplate(Operacao operacao, StatusOperacao novoStatus)
        {
            operacao.Status = novoStatus;
        }

        public void Creditar(Conta conta, decimal valor)
        {
            Action<Conta> acaoPrincipal = conta =>
            {
                conta.Saldo += valor;
            };

            AlterarValoresTemplate(conta, valor, acaoPrincipal);
        }

        public void Debitar(Conta conta, decimal valor)
        {
            Action<Conta> acaoPrincipal = conta =>
            {
                ValidarSaldo(conta, valor);
                conta.Saldo -= valor;
            };

            AlterarValoresTemplate(conta, valor, acaoPrincipal);
        }

        public void AlterarValoresTemplate(Conta conta, decimal valor, Action<Conta> acaoPrincipal)
        {
            ValidarValor(valor);
            acaoPrincipal(conta);
        }

        public void ValidarValor(decimal valor)
        {
            ValidarArgumentoTemplate(valor <= 0, "O valor da operação deve ser superior a zero!");
        }

        public void ValidarSaldo(Conta conta, decimal valor)
        {
            ValidarArgumentoTemplate(conta.Saldo < valor, "O saldo da conta é insuficiente para realizar a operação!");
        }

        public void ValidarArgumentoTemplate(bool condicao, string mensagem)
        {
            if (condicao)
                throw new CarteiraDigitalException(mensagem);
        }
    }
}
