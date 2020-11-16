using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class ContaServico : IContaServico
    {
        private readonly IContaRepositorio contaRepositorio;

        public ContaServico(IContaRepositorio contaRepositorio)
        {
            this.contaRepositorio = contaRepositorio;
        }

        public void Cadastrar(string usuarioTitular)
        {
            var conta = new Conta(usuarioTitular);
            contaRepositorio.Post(conta);
        }

        public int ObterIdPeloTitular(string usuarioTitular)
        {
            return contaRepositorio.ObterIdPeloTitular(usuarioTitular);
        }

        public decimal ObterSaldoAtual(int contaId)
        {
            return contaRepositorio.ObterSaldoAtual(contaId);
        }

        public Conta ObterConta(int contaId)
        {
            ValidarConta(contaId);
            return contaRepositorio.Get(contaId);
        }

        public MovimentacaoDto ObterMovimentacao(int contaId, DateTime dataInicial, DateTime dataFinal)
        {
            return contaRepositorio.ObterMovimentacao(contaId, dataInicial, dataFinal);
        }

        public void ValidarConta(int contaId)
        {
            if(!contaRepositorio.Any(contaId))
                throw new ArgumentException("A conta informada é inválida!");
        }

        public void VincularCashIn(Conta conta, CashIn cashIn)
        {
            conta.CashIns.Add(cashIn);
            contaRepositorio.Update(conta);
        }

        public void VincularCashOut(Conta conta, CashOut cashOut)
        {
            conta.CashOuts.Add(cashOut);
            contaRepositorio.Update(conta);
        }

        public void VincularTransferencia(Conta conta, Transferencia transferencia)
        {
            conta.Transferencias.Add(transferencia);
            contaRepositorio.Update(conta);
        }
    }
}
