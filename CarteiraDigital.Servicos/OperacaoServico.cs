using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class OperacaoServico : IOperacaoServico
    {
        private readonly IConfiguracaoServico configuracaoServico;

        public OperacaoServico(IConfiguracaoServico configuracaoServico)
        {
            this.configuracaoServico = configuracaoServico;
        }

        public bool PodeAlterarStatus(Operacao operacao)
        {
            return operacao.Status != StatusOperacao.ComErro;
        }

        public void MarcarEfetivada(Operacao operacao)
        {
            AlterarStatusTemplate(operacao, StatusOperacao.Efetivada);
        }

        public void MarcarComErro(Operacao operacao, string erro)
        {
            operacao.Erro = erro;
            AlterarStatusTemplate(operacao, StatusOperacao.ComErro);
        }

        public void AlterarStatusTemplate(Operacao operacao, StatusOperacao novoStatus)
        {
            if (!PodeAlterarStatus(operacao))
                throw new CarteiraDigitalException($"Não é possível alterar o status de uma operação {operacao.Status.ObterDescricao()}!");

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

        public void ValidarDescricao(string descricao)
        {
            var maximo = configuracaoServico.ObterTamanhoMaximoDescricao();

            ValidarArgumentoTemplate(!string.IsNullOrEmpty(descricao) && descricao.Length > maximo,
                                     $"A descrição não pode ter mais que {maximo} caracteres!");
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
