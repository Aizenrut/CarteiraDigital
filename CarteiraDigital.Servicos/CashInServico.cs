using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class CashInServico : ICashInServico
    {
        private readonly ICashInRepositorio cashInRepositorio;
        private readonly IOperacaoServico operacaoServico;
        private readonly IContaServico contaServico;

        public CashInServico(ICashInRepositorio cashInRepositorio,
                             IOperacaoServico operacaoServico,
                             IContaServico contaServico)
        {
            this.cashInRepositorio = cashInRepositorio;
            this.operacaoServico = operacaoServico;
            this.contaServico = contaServico;
        }

        public bool EhPrimeiroCashIn(int contaId)
        {
            return !cashInRepositorio.Any(x => x.ContaId == contaId);
        }

        public void Efetivar(OperacaoUnariaDto dto)
        {
            var conta = contaServico.ObterConta(dto.ContaId);

            var cashIn = GerarCashIn(conta, dto.Valor, dto.Descricao);
            operacaoServico.Creditar(conta, cashIn.Valor);
            cashInRepositorio.Post(cashIn);

            contaServico.VincularCashIn(conta, cashIn);
        }

        public CashIn GerarCashIn(Conta conta, decimal valor, string descricao)
        {
            if (EhPrimeiroCashIn(conta.Id))
                valor *= 1.10m;

            return new CashIn(valor, descricao, conta.Saldo);
        }
    }
}
