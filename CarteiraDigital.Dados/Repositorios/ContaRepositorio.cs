using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Models;
using CarteiraDigital.Models.Enumeracoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarteiraDigital.Dados.Repositorios
{
    public class ContaRepositorio : Repositorio<Conta>, IContaRepositorio
    {
        private readonly IOperacaoExpressao operacaoExpressao;

        public ContaRepositorio(CarteiraDigitalContext context, IOperacaoExpressao operacaoExpressao) : base(context)
        {
            this.operacaoExpressao = operacaoExpressao;
        }

        public int ObterContaDoTitular(string usuarioTitular)
        {
            return context.Contas.Where(x => x.UsuarioTitular == usuarioTitular)
                                 .Select(x => x.Id)
                                 .FirstOrDefault();
        }

        public decimal ObterSaldoAtual(int contaId)
        {
            return context.Contas.Where(x => x.Id == contaId)
                                 .Select(x => x.Saldo)
                                 .FirstOrDefault();
        }

        public bool PossuiOperacoes(ICollection<OperacaoDto> operacoes)
        {
            return operacoes != null && operacoes.Any();
        }

        public decimal ObterSaldoFinal(ICollection<OperacaoDto> operacoes)
        {
            Func<ICollection<OperacaoDto>, decimal> acaoPrincipal = (operacoes) =>
            {
                var operacaoDoTopo = operacoes.First();
                decimal saldoFinal = operacaoDoTopo.SaldoAnterior;

                if (operacoes.First().TipoMovimentacao == TipoMovimentacao.Entrada)
                    saldoFinal += operacaoDoTopo.Valor;
                else
                    saldoFinal -= operacaoDoTopo.Valor;

                return saldoFinal;
            };

            return ObterSaldoTemplate(operacoes, acaoPrincipal);
        }

        public decimal ObterSaldoInicial(ICollection<OperacaoDto> operacoes)
        {
            Func<ICollection<OperacaoDto>, decimal> acaoPrincipal = (operacoes) =>
            {
                return operacoes.Last().SaldoAnterior;
            };

            return ObterSaldoTemplate(operacoes, acaoPrincipal);
        }

        public decimal ObterSaldoTemplate(ICollection<OperacaoDto> operacoes, Func<ICollection<OperacaoDto>, decimal> acaoPrincipal)
        {
            if (!PossuiOperacoes(operacoes))
                return 0;

            return acaoPrincipal(operacoes);
        }

        public MovimentacaoDto ObterMovimentacao(int contaId, DateTime dataInicial, DateTime dataFinal)
        {
            if (dataFinal == default)
                dataFinal = DateTime.Now;

            var operacoes = context.CashIns.Where(operacaoExpressao.DaContaNoPeriodo<CashIn>(contaId, dataInicial, dataFinal))
                                           .Select(cashIn => new OperacaoDto
                                           {
                                               Data = cashIn.Data,
                                               Status = cashIn.Status,
                                               Valor = cashIn.Valor,
                                               Descricao = cashIn.Descricao,
                                               TipoMovimentacao = TipoMovimentacao.Entrada,
                                               TipoOperacao = TipoOperacao.CashIn,
                                               SaldoAnterior = cashIn.SaldoAnterior,
                                               Erro = cashIn.Erro
                                           })
                                   .Union(context.CashOuts.Where(operacaoExpressao.DaContaNoPeriodo<CashOut>(contaId, dataInicial, dataFinal))
                                                          .Select(cashOut => new OperacaoDto
                                                          {
                                                              Data = cashOut.Data,
                                                              Status = cashOut.Status,
                                                              Valor = cashOut.Valor,
                                                              Descricao = cashOut.Descricao,
                                                              TipoMovimentacao = TipoMovimentacao.Saida,
                                                              TipoOperacao = TipoOperacao.CashOut,
                                                              SaldoAnterior = cashOut.SaldoAnterior,
                                                              Erro = cashOut.Erro
                                                          })
                                   .Union(context.Transferencias.Where(operacaoExpressao.DaContaNoPeriodo<Transferencia>(contaId, dataInicial, dataFinal))
                                                                .Select(transferencia => new OperacaoDto
                                                                {
                                                                    Data = transferencia.Data,
                                                                    Status = transferencia.Status,
                                                                    Valor = transferencia.Valor,
                                                                    Descricao = transferencia.Descricao,
                                                                    TipoMovimentacao = transferencia.TipoMovimentacao,
                                                                    TipoOperacao = TipoOperacao.Transferencia,
                                                                    SaldoAnterior = transferencia.SaldoAnterior,
                                                                    Erro = transferencia.Erro
                                                                })))
                                    .OrderByDescending(x => x.Data)
                                    .ToList();

            var operacoesEfetivadas = operacoes.Where(operacaoExpressao.Efetivada());

            return new MovimentacaoDto(ObterSaldoAtual(contaId), ObterSaldoFinal(operacoes), ObterSaldoInicial(operacoes), operacoes);
        }
    }
}
