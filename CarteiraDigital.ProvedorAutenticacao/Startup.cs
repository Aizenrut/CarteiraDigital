using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IdentityModel.Tokens.Jwt;
using CarteiraDigital.ProvedorAutenticacao.Dados;
using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using CarteiraDigital.Servicos;
using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.ProvedorAutenticacao.Builders;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Linq;
using CarteiraDigital.ProvedorAutenticacao.Extensions;

namespace CarteiraDigital.ProvedorAutenticacao
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => 
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ErroRespostaFilter>();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddApiVersioning();

            services.AddDbContext<CarteiraDigitalAutorizacaoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CarteiraDigital"));
            });

            services.AddDbContext<CarteiraDigitalContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CarteiraDigital"));
            });

            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<CarteiraDigitalAutorizacaoContext>();

            services.AddTransient<IUsuarioBuilder, UsuarioBuilder>();
            services.AddTransient<IOperacaoExpressao, OperacaoExpressao>();
            services.AddTransient<IContaRepositorio, ContaRepositorio>();
            services.AddTransient<ILogRepositorio, LogRepositorio>();
            services.AddTransient<IConfiguracaoServico, ConfiguracaoServico>();
            services.AddTransient<ILoginServico, LoginServico>();
            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.AddTransient<IContaServico, ContaServico>();
            services.AddTransient<IValidacaoDocumentosServico, ValidacaoDocumentosServico>();
            services.AddTransient<ILogServico, LogServico>();
            services.TryAddTransient<JwtSecurityTokenHandler>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "Provedor de Autenticação",
                    Version = "1.0",
                    Description = "API utilizada para realizar as operações referentes ao usuário e autenticação."
                });

                var arquivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var caminho = Path.Combine(AppContext.BaseDirectory, arquivo);
                options.IncludeXmlComments(caminho);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCulturas();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/Swagger/v1.0/swagger.json", "Versão 1.0");
                options.RoutePrefix = string.Empty;
            });

            app.UseApiVersioning();
            app.UseMvc();
        }
    }
}
