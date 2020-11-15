using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashInServico : IOperacaoStrategy<OperacaoUnariaDto>
    {
        bool EhPrimeiroCashIn(int contaId);
        CashIn GerarCashIn(Conta conta, decimal valor, string descricao);
    }
}
