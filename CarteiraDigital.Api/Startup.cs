using CarteiraDigital.Api.Models;
using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
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

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "API da Carteira Digital",
                    Version = "1.0",
                    Description = "API utilizada para realizar opera��es da Carteira Digital."
                });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Autentica��o utilizando o esquema Bearer. Ex.: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AdicionarAutenticacaoOperationFilter>();

                var arquivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var caminho = Path.Combine(AppContext.BaseDirectory, arquivo);
                options.IncludeXmlComments(caminho);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/Swagger/v1.0/swagger.json", "Vers�o 1.0");
                options.RoutePrefix = string.Empty;
            });

            app.UseApiVersioning();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
