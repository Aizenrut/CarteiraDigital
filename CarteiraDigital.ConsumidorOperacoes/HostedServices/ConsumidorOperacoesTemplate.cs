using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Models;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace CarteiraDigital.ConsumidorOperacoes.HostedServices
{
    public abstract class ConsumidorOperacoesTemplate<TOperacao, TEfetivar, TGerar> : BackgroundService,
        IConsumidorOperacoes<TOperacao, TEfetivar, TGerar> where TOperacao : Operacao
                                                           where TEfetivar : struct
                                                           where TGerar : struct
    {
        private readonly IConsumidorOperacoesServico<TOperacao, TEfetivar> consumidorOperacoes;
        private readonly IModel canal;
        private readonly string fila;

        public ConsumidorOperacoesTemplate(IConsumidorOperacoesServico<TOperacao, TEfetivar> consumidorOperacoes,
                                           IModel canal,
                                           string fila)
        {
            this.consumidorOperacoes = consumidorOperacoes;
            this.canal = canal;
            this.fila = fila;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumidor = new EventingBasicConsumer(canal);

            consumidor.Received += ConsumeAsync;

            canal.BasicConsume(queue: fila,
                               autoAck: false,
                               consumer: consumidor);

            return Task.CompletedTask;
        }

        private async void ConsumeAsync(object model, BasicDeliverEventArgs eventArgs)
        {
            await new TaskFactory().StartNew(() => consumidorOperacoes.Consumir(model, eventArgs));
        }

        public override void Dispose()
        {
            canal.Dispose();
            base.Dispose();
        }
    }
}
