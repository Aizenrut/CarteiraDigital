using CarteiraDigital.Models;

namespace CarteiraDigital.Servicos
{
    public interface ICashOutServico : IOperacaoStrategy<EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
    }
}
