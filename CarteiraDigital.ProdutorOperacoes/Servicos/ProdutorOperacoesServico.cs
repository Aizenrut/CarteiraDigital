using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CarteiraDigital.ProdutorOperacoes.Servicos
{
    public class ProdutorOperacoesServico<T> : IProdutorOperacoesServico<T> where T : struct
    {
        private readonly IModel channel;

        public ProdutorOperacoesServico(IModel channel)
        {
            this.channel = channel;
        }

        public void Produzir(T operacao, string routingKey)
        {
            var mensagem = JsonConvert.SerializeObject(operacao);
            var bytes = Encoding.UTF8.GetBytes(mensagem);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: bytes);

            channel.Dispose();
        }
    }
}
