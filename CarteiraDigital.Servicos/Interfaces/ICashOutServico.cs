using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashOutServico
    {
        void Efetivar(OperacaoUnariaDto dto);
        CashOut GerarCashOut(Conta conta, decimal valor, string descricao);
    }
}
