using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashInServico : IOperacaoStrategy<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
        bool EhPrimeiroCashIn(int contaId);
        decimal ObterBonificacao(int contaId, decimal valor);
    }
}
