using CarteiraDigital.Models;
using CarteiraDigital.ProdutorOperacoes.Servicos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CarteiraDigital.ProdutorOperacoes
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
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

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

            services.AddTransient<IProdutorOperacoesServico<EfetivarOperacaoUnariaDto>, ProdutorOperacoesServico<EfetivarOperacaoUnariaDto>>();
            services.AddTransient<IProdutorOperacoesServico<EfetivarOperacaoBinariaDto>, ProdutorOperacoesServico<EfetivarOperacaoBinariaDto>>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
