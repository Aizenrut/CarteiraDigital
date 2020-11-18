using CarteiraDigital.ConsumidorOperacoes.HostedServices;
using CarteiraDigital.ConsumidorOperacoes.Servicos;
using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using CarteiraDigital.Servicos.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using System;
using System.Net.Http;

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

            services.AddDbContext<CarteiraDigitalContext>(options =>
            {
                options.UseSqlServer(configuracao.GetConnectionString("CarteiraDigital"));
            }, ServiceLifetime.Singleton);

            services.TryAddTransient(factory =>
            {
                return factory.GetRequiredService<IHttpClientFactory>().CreateClient(string.Empty);
            });

            services.AddHttpClient<IProdutorOperacoesClient, ProdutorOperacoesClient>();

            services.AddTransient<IContaRepositorio, ContaRepositorio>();
            services.AddTransient<ICashInRepositorio, CashInRepositorio>();
            services.AddTransient<ICashOutRepositorio, CashOutRepositorio>();
            services.AddTransient<ITransferenciaRepositorio, TransferenciaRepositorio>();

            services.AddTransient<IOperacaoExpressao, OperacaoExpressao>();
            services.AddTransient<IConfiguracaoServico, ConfiguracaoServico>();
            services.AddTransient<ITransacaoServico, TransacaoServico>();
            services.AddTransient<IProdutorOperacoesClient, ProdutorOperacoesClient>();

            services.AddTransient<IOperacaoServico, OperacaoServico>();
            services.AddTransient<IContaServico, ContaServico>();
            services.AddTransient<ICashInServico, CashInServico>();
            services.AddTransient<ICashOutServico, CashOutServico>();
            services.AddTransient<ITransferenciaServico, TransferenciaServico>();

            services.AddTransient<IOperacaoStrategy<CashIn, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>, CashInServico>();
            services.AddTransient<IOperacaoStrategy<CashOut, EfetivarOperacaoUnariaDto, OperacaoUnariaDto>, CashOutServico>();
            services.AddTransient<IOperacaoStrategy<Transferencia, EfetivarOperacaoBinariaDto, OperacaoBinariaDto>, TransferenciaServico>();
            
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
