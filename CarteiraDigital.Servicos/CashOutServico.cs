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

        public void Efetivar(EfetivarOperacaoUnariaDto dto)
        {
            var cashOut = cashOutRepositorio.Get(dto.OperacaoId);

            try
            {
                var conta = contaServico.ObterConta(cashOut.ContaId);

                using (var transacao = transacaoServico.GerarNova())
                {
                    operacaoServico.Debitar(conta, cashOut.Valor + cashOut.ValorTaxa);
                    operacaoServico.MarcarEfetivada(cashOut);

                    transacao.Finalizar();
                }
            }
            catch (CarteiraDigitalException e)
            {
                operacaoServico.MarcarComErro(cashOut, e.Message);
            }

            cashOutRepositorio.Update(cashOut);
        }

        public void Gerar(OperacaoUnariaDto dto)
        {
            var conta = contaServico.ObterConta(dto.ContaId);
            var valorTaxa = dto.Valor * configuracaoServico.ObterPercentualTaxa();

            var cashOut = new CashOut(conta.Id, dto.Valor, dto.Descricao, conta.Saldo, valorTaxa);

            operacaoServico.MarcarPendente(cashOut);

            cashOutRepositorio.Post(cashOut);
            contaServico.VincularCashOut(conta, cashOut);
        }
    }
}
