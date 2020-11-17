using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Models;
using RabbitMQ.Client;

namespace CarteiraDigital.ConsumidorOperacoes.HostedServices
{
    public class ConsumidorCashIns : ConsumidorOperacoesTemplate<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>
    {
        public ConsumidorCashIns(IConsumidorOperacoesServico<CashIn, EfetivarOperacaoUnariaDto> consumidorOperacoes,
                                 IModel canal)
            : base(consumidorOperacoes, canal, "cashIns")
        {
        }
    }
}
