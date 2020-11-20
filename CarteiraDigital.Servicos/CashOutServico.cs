using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos.Clients;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos
{
    public class CashOutServico : ICashOutServico
    {
        private readonly ICashOutRepositorio cashOutRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;
        private readonly IConfiguracaoServico configuracaoServico;
        private readonly ITransacaoServico transacaoServico;
        private readonly IProdutorOperacoesClient produtorClient;

        public CashOutServico(ICashOutRepositorio cashOutRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico,
                             IConfiguracaoServico configuracaoServico,
                             ITransacaoServico transacaoServico,
                             IProdutorOperacoesClient produtorClient)
        {
            this.cashOutRepositorio = cashOutRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
            this.configuracaoServico = configuracaoServico;
            this.transacaoServico = transacaoServico;
            this.produtorClient = produtorClient;
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

        public async Task Gerar(OperacaoUnariaDto dto)
        {
            operacaoServico.ValidarDescricao(dto.Descricao);

            var conta = contaServico.ObterConta(dto.ContaId);
            var valorTaxa = dto.Valor * configuracaoServico.ObterPercentualTaxa();

            var cashOut = new CashOut(conta.Id, dto.Valor, dto.Descricao, conta.Saldo, valorTaxa);

            cashOutRepositorio.Post(cashOut);
            contaServico.VincularCashOut(conta, cashOut);

            await produtorClient.EnfileirarCashOut(new EfetivarOperacaoUnariaDto(cashOut.Id));
        }
    }
}
