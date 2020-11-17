using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Models;
using RabbitMQ.Client;

namespace CarteiraDigital.ConsumidorOperacoes.HostedServices
{
    public class ConsumidorTransferencias : ConsumidorOperacoesTemplate<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>
    {
        public ConsumidorTransferencias(IConsumidorOperacoesServico<Transferencia, EfetivarOperacaoBinariaDto> consumidorOperacoes,
                                        IModel canal) 
            : base(consumidorOperacoes, canal, "transferencias")
        {
        }
    }
}
