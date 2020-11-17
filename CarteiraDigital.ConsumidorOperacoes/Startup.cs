using CarteiraDigital.ConsumidorOperacoes.HostedServices;
using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CarteiraDigital.ConsumidorOperacoes
{
    public class Startup
    {
        private IConfiguration configuracao;
        private IConnection conexaoAtual;

        public Startup(IConfiguration configuracao)
        {
            this.configuracao = configuracao;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(factory =>
            {
                var rabbitMqSection = configuracao.GetSection("RabbitMQ");

                return new ConnectionFactory()
                {
                    HostName = rabbitMqSection["HostName"],
                    UserName = rabbitMqSection["UserName"],
                    Password = rabbitMqSection["Password"]
                };
            });

            services.AddTransient(factory =>
            {
                if (conexaoAtual == null || !conexaoAtual.IsOpen)
                {
                    var fabrica = factory.GetService<ConnectionFactory>();
                    conexaoAtual = fabrica.CreateConnection();
                }

                return conexaoAtual;
            });

            services.AddTransient(factory =>
            {
                var conexao = factory.GetService<IConnection>();
                return conexao.CreateModel();
            });

            services.AddTransient<IConsumidorOperacoesServico<CashIn,  EfetivarOperacaoUnariaDto>, ConsumidorOperacoesServico<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>>();
            services.AddTransient<IConsumidorOperacoesServico<CashOut, EfetivarOperacaoUnariaDto>, ConsumidorOperacoesServico<CashOut, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>>();
            services.AddTransient<IConsumidorOperacoesServico<Transferencia, EfetivarOperacaoBinariaDto>, ConsumidorOperacoesServico<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>>();
            services.AddTransient<IConsumidorOperacoes<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>, ConsumidorCashIns>();
            services.AddTransient<IConsumidorOperacoes<CashOut, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>, ConsumidorCashOuts>();
            services.AddTransient<IConsumidorOperacoes<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>, ConsumidorTransferencias>();

            services.AddHostedService<ConsumidorCashIns>();
            services.AddHostedService<ConsumidorCashOuts>();
            services.AddHostedService<ConsumidorTransferencias>();
        }

        public void Configure()
        {
        }
    }
}
