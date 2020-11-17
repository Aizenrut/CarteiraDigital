using CarteiraDigital.Models;
using Microsoft.Extensions.Hosting;

namespace CarteiraDigital.ConsumidorOperacoes.HostedServices
{
    public interface IConsumidorOperacoes<TOperacao, TEfetivar, TGerar> : IHostedService where TOperacao : Operacao
                                                                                         where TEfetivar : struct
                                                                                         where TGerar : struct
    {
    }
}
