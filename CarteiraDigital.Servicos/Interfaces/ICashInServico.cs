using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashInServico : IOperacaoStrategy<EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
        bool EhPrimeiroCashIn(int contaId);
        decimal ObterValorComBonificacao(int contaId, decimal valor);
    }
}
