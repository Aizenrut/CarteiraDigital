using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos.Clients;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos
{
    public class CashInServico : ICashInServico
    {
        private readonly ICashInRepositorio cashInRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;
        private readonly IConfiguracaoServico configuracaoServico;
        private readonly ITransacaoServico transacaoServico;
        private readonly IProdutorOperacoesClient produtorClient;

        public CashInServico(ICashInRepositorio cashInRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico,
                             IConfiguracaoServico configuracaoServico,
                             ITransacaoServico transacaoServico,
                             IProdutorOperacoesClient produtorClient)
        {
            this.cashInRepositorio = cashInRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
            this.configuracaoServico = configuracaoServico;
            this.transacaoServico = transacaoServico;
            this.produtorClient = produtorClient;
        }

        public bool EhPrimeiroCashIn(int contaId)
        {
            return !cashInRepositorio.ExisteCashInEfetivado(contaId);
        }

        public void Efetivar(EfetivarOperacaoUnariaDto dto)
        {
            var cashIn = cashInRepositorio.Get(dto.OperacaoId);

            try
            {
                var conta = contaServico.ObterConta(cashIn.ContaId);

                using (var transacao = transacaoServico.GerarNova())
                {
                    operacaoServico.Creditar(conta, cashIn.Valor + cashIn.ValorBonificacao);
                    operacaoServico.MarcarEfetivada(cashIn);

                    transacao.Finalizar();
                }
            }
            catch (CarteiraDigitalException e)
            {
                operacaoServico.MarcarComErro(cashIn, e.Message);
            }

            cashInRepositorio.Update(cashIn);
        }

        public async Task Gerar(OperacaoUnariaDto dto)
        {
            operacaoServico.ValidarDescricao(dto.Descricao);

            var conta = contaServico.ObterConta(dto.ContaId);
            var bonificacao = ObterBonificacao(conta.Id, dto.Valor);

            var cashIn = new CashIn(conta.Id, dto.Valor, dto.Descricao, conta.Saldo, bonificacao);

            cashInRepositorio.Post(cashIn);
            contaServico.VincularCashIn(conta, cashIn);

            await produtorClient.EnfileirarCashIn(new EfetivarOperacaoUnariaDto(cashIn.Id));
        }

        public decimal ObterBonificacao(int contaId, decimal valor)
        {
            if (EhPrimeiroCashIn(contaId))
                return valor * configuracaoServico.ObterPercentualBonificacao();

            return 0;
        }
    }
}
