using CarteiraDigital.Models;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos
{
    public interface IOperacaoStrategy<TOperacao, TDtoEfetivar, TDtoGerar> where TOperacao : Operacao
                                                                           where TDtoEfetivar : struct
                                                                           where TDtoGerar : struct
    {
        void Efetivar(TDtoEfetivar operacao);
        Task Gerar(TDtoGerar dto);
    }
}
