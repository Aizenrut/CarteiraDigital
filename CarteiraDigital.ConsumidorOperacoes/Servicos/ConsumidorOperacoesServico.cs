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
        private readonly IOperacaoStrategy<TOperacao, TEfetivar, TGerar> strategy;
        private readonly ILogger<ConsumidorOperacoesServico<TOperacao, TEfetivar, TGerar>> logger;

        public ConsumidorOperacoesServico(IOperacaoStrategy<TOperacao, TEfetivar, TGerar> strategy,
                                          ILogger<ConsumidorOperacoesServico<TOperacao, TEfetivar, TGerar>> logger)
        {
            this.strategy = strategy;
            this.logger = logger;
        }

        public void Consumir(IModel canal, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                var operacao = JsonConvert.DeserializeObject<TEfetivar>(mensagem);

                strategy.Efetivar(operacao);
                canal.BasicAck(eventArgs.DeliveryTag, false);
                logger.LogInformation("Operação processada.");
            }
            catch (Exception e)
            {
                canal.BasicNack(eventArgs.DeliveryTag, false, true);
                logger.LogError($"Erro: {e.Message}", e);
            }
        }
    }
}
