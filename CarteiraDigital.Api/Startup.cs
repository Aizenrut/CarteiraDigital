using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CarteiraDigital.Api
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
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddApiVersioning();

            services.AddDbContext<CarteiraDigitalContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CarteiraDigital"));
            });

            var schemesSection = configuration.GetSection("Schemes");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = schemesSection["Authentication"];
                options.DefaultChallengeScheme = schemesSection["Challenge"];
            }).AddJwtBearer(schemesSection["Authentication"], options =>
            {
                var keysSection = configuration.GetSection("Keys");
                var claimsSection = configuration.GetSection("Claims");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = claimsSection["Iss"],
                    ValidAudience = claimsSection["Aud"],
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keysSection["JwtBearer"]))
                };
            });

            services.AddTransient<IJwtServico, JwtServico>();
            services.AddTransient<IRequisicaoServico, RequisicaoServico>();
            services.AddTransient<ITransacaoServico, TransacaoServico>();
            services.AddTransient<IOperacaoExpressao, OperacaoExpressao>();
            services.AddTransient<IContaRepositorio, ContaRepositorio>();
            services.AddTransient<ICashInRepositorio, CashInRepositorio>();
            services.AddTransient<ICashOutRepositorio, CashOutRepositorio>();
            services.AddTransient<ITransferenciaRepositorio, TransferenciaRepositorio>();
            services.AddTransient<IConfiguracaoServico, ConfiguracaoServico>();
            services.AddTransient<IContaServico, ContaServico>();
            services.AddTransient<IOperacaoServico, OperacaoServico>();
            services.AddTransient<ICashInServico, CashInServico>();
            services.AddTransient<ICashOutServico, CashOutServico>();
            services.AddTransient<ITransferenciaServico, TransferenciaServico>();
            services.AddTransient<IContaServico, ContaServico>();
            services.AddTransient<IValidacaoDocumentosServico, ValidacaoDocumentosServico>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseApiVersioning();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
