using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public interface IContaServico
    {
        void Cadastrar(string usuarioTitular);
        int ObterIdPeloTitular(string usuarioTitular);
        decimal ObterSaldoAtual(int contaId);
        Conta ObterConta(int contaId);
        void ValidarConta(int contaId);
        void VincularCashIn(Conta conta, CashIn cashIn);
        void VincularCashOut(Conta conta, CashOut cashOut);
        void VincularTransferencia(Conta conta, Transferencia transferencia);
        MovimentacaoDto ObterMovimentacao(int contaId, DateTime dataInicial, DateTime dataFinal);
    }
}
