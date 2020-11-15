using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashOutServico : IOperacaoStrategy<OperacaoUnariaDto>
    {
        CashOut GerarCashOut(Conta conta, decimal valor, string descricao);
    }
}
