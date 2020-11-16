using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CarteiraDigital.Servicos
{
    public class ConfiguracaoServico : IConfiguracaoServico
    {
        private const decimal PERCENTUAL_BONIFICACAO = 0.1m;
        private const decimal PERCENTUAL_TAXA = 0.01m;

        private readonly IConfiguration configuracao;

        public ConfiguracaoServico(IConfiguration configuracao)
        {
            this.configuracao = configuracao;
        }

        public byte ObterIdadeMinima()
        {
            var regiaoAtual = System.Globalization.RegionInfo.CurrentRegion.ThreeLetterISORegionName;
            var idadeMinimaSection = configuracao.GetSection("Usuario")
                                                 .GetSection("IdadeMinima");
            byte idadeMinima = 0;

            if (idadeMinimaSection.GetChildren().Any(x => x.Key.Equals(regiaoAtual, StringComparison.InvariantCultureIgnoreCase)))
                idadeMinima = Convert.ToByte(idadeMinimaSection[regiaoAtual]);

            return idadeMinima;
        }

        public decimal ObterPercentualBonificacao()
        {
            return PERCENTUAL_BONIFICACAO;
        }

        public decimal ObterPercentualTaxa()
        {
            return PERCENTUAL_TAXA;
        }
    }
}
