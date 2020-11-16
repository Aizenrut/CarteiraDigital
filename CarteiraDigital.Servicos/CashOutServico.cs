using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public class CashOutServico : ICashOutServico
    {
        private readonly ICashOutRepositorio cashOutRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;
        private readonly IConfiguracaoServico configuracaoServico;
        private readonly ITransacaoServico transacaoServico;

        public CashOutServico(ICashOutRepositorio cashOutRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico,
                             IConfiguracaoServico configuracaoServico,
                             ITransacaoServico transacaoServico)
        {
            this.cashOutRepositorio = cashOutRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
            this.configuracaoServico = configuracaoServico;
            this.transacaoServico = transacaoServico;
        }

        public void Efetivar(OperacaoUnariaDto dto)
        {
            var conta = contaServico.ObterConta(dto.ContaId);
            var cashOut = GerarCashOut(conta, dto.Valor, dto.Descricao);

            using (var transacao = transacaoServico.GerarNova())
            {
                operacaoServico.Debitar(conta, cashOut.Valor + cashOut.ValorTaxa);
                cashOutRepositorio.Post(cashOut);
                contaServico.VincularCashOut(conta, cashOut);

                transacao.Finalizar();
            }
        }

        public CashOut GerarCashOut(Conta conta, decimal valor, string descricao)
        {
            var valorTaxa = valor * configuracaoServico.ObterPercentualTaxa();
            return new CashOut(conta.Id, valor, descricao, conta.Saldo, valorTaxa);
        }
    }
}
