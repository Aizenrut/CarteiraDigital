using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public class CashInServico : ICashInServico
    {
        private readonly ICashInRepositorio cashInRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;
        private readonly IConfiguracaoServico configuracaoServico;
        private readonly ITransacaoServico transacaoServico;

        public CashInServico(ICashInRepositorio cashInRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico,
                             IConfiguracaoServico configuracaoServico,
                             ITransacaoServico transacaoServico)
        {
            this.cashInRepositorio = cashInRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
            this.configuracaoServico = configuracaoServico;
            this.transacaoServico = transacaoServico;
        }

        public bool EhPrimeiroCashIn(int contaId)
        {
            return !cashInRepositorio.Any(x => x.ContaId == contaId);
        }

        public void Efetivar(OperacaoUnariaDto dto)
        {
            var conta = contaServico.ObterConta(dto.ContaId);
            var cashIn = GerarCashIn(conta, dto.Valor, dto.Descricao);

            using (var transacao = transacaoServico.GerarNova())
            {
                operacaoServico.Creditar(conta, cashIn.Valor);
                cashInRepositorio.Post(cashIn);
                contaServico.VincularCashIn(conta, cashIn);

                transacao.Finalizar();
            }
        }

        public CashIn GerarCashIn(Conta conta, decimal valor, string descricao)
        {
            if (EhPrimeiroCashIn(conta.Id))
                valor *= 1 + configuracaoServico.ObterPercentualBonificacao();

            return new CashIn(conta.Id, valor, descricao, conta.Saldo);
        }
    }
}
