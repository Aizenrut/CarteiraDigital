using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public class CashOutServico : ICashOutServico
    {
        private readonly ICashOutRepositorio cashOutRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;

        public CashOutServico(ICashOutRepositorio cashOutRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico)
        {
            this.cashOutRepositorio = cashOutRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
        }

        public void Efetivar(OperacaoUnariaDto dto)
        {
            var conta = contaServico.ObterConta(dto.ContaId);

            var cashOut = GerarCashOut(conta, dto.Valor, dto.Descricao);
            operacaoServico.Debitar(conta, cashOut.Valor + cashOut.ValorTaxa);
            cashOutRepositorio.Post(cashOut);

            contaServico.VincularCashOut(conta, cashOut);
        }

        public CashOut GerarCashOut(Conta conta, decimal valor, string descricao)
        {
            return new CashOut(valor, descricao, conta.Saldo, valor * 0.01m);
        }
    }
}
