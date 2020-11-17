using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace CarteiraDigital.ConsumidorOperacoes.Servicos
{
    public class ConsumidorOperacoesServico<TOperacao, TEfetivar, TGerar> : IConsumidorOperacoesServico<TOperacao, TEfetivar> where TOperacao  : Operacao
                                                                                                                              where TEfetivar : struct
                                                                                                                              where TGerar : struct
    {
        private readonly IModel channel;
        private readonly IOperacaoStrategy<TOperacao, TEfetivar, TGerar> strategy;
        private readonly ILogger<ConsumidorOperacoesServico<TOperacao, TEfetivar, TGerar>> logger;

        public ConsumidorOperacoesServico(IModel channel,
                                          IOperacaoStrategy<TOperacao, TEfetivar, TGerar> strategy,
                                          ILogger<ConsumidorOperacoesServico<TOperacao, TEfetivar, TGerar>> logger)
        {
            this.channel = channel;
            this.strategy = strategy;
            this.logger = logger;
        }

        public void Consumir(object model, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                var operacao = JsonConvert.DeserializeObject<TEfetivar>(mensagem);

                strategy.Efetivar(operacao);
                channel.BasicAck(eventArgs.DeliveryTag, false);
                logger.LogInformation($"Operação efetivada.");
            }
            catch (Exception e)
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, true);
                logger.LogError($"Erro: {e.Message}", e);
            }
        }
    }
}
