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
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddDbContext<CarteiraDigitalAutorizacaoContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CarteiraDigital"));
            });

            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<CarteiraDigitalAutorizacaoContext>();

            services.AddTransient<ILoginServico, LoginServico>();
            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.TryAddTransient<JwtSecurityTokenHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
