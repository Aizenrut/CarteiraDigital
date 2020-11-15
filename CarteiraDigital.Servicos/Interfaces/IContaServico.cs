using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface IContaServico
    {
        Conta ObterConta(int contaId);
        void ValidarConta(int contaId);
        void VincularCashIn(Conta conta, CashIn cashIn);
        void VincularCashOut(Conta conta, CashOut cashOut);
        void VincularTransferencia(Conta conta, Transferencia transferencia);
    }
}
