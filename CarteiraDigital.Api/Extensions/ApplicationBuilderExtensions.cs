using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;

namespace CarteiraDigital.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCulturas(this IApplicationBuilder app)
        {
            var configuracaoServico = app.ApplicationServices.GetService<IConfiguracaoServico>();

            var culturaPadrao = configuracaoServico.ObterCulturaPadrao();
            var culturasSuportadas = configuracaoServico.ObterCulturasSuportadas()
                                                        .Select(cultura => new CultureInfo(cultura))
                                                        .ToArray();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culturaPadrao, culturaPadrao),
                SupportedCultures = culturasSuportadas,
                SupportedUICultures = culturasSuportadas
            });

            return app;
        }
    }
}
