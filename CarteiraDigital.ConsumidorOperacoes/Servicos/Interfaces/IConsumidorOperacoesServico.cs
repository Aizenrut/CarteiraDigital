using CarteiraDigital.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CarteiraDigital.ConsumidorOperacoes.Servicos
{
    public interface IConsumidorOperacoesServico<TOperacao, TEfetivar> where TOperacao : Operacao
                                                                       where TEfetivar : struct
    {
        void Consumir(IModel canal, BasicDeliverEventArgs eventArgs);
    }
}
