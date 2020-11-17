using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashInServico : IOperacaoStrategy<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
        bool EhPrimeiroCashIn(int contaId);
        decimal ObterValorComBonificacao(int contaId, decimal valor);
    }
}
