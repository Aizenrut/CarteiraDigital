using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashOutServico : IOperacaoStrategy<CashOut, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
    }
}
