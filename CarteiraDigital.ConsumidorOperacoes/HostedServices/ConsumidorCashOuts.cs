using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Models;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CarteiraDigital.ConsumidorOperacoes.HostedServices
{
    public class ConsumidorCashOuts : ConsumidorOperacoesTemplate<CashOut, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
        public ConsumidorCashOuts(IConsumidorOperacoesServico<CashOut, EfetivarOperacaoUnariaDto> consumidorOperacoes,
                                  IModel canal)
            : base(consumidorOperacoes, canal, "cashOuts")
        {
        }
    }
}
