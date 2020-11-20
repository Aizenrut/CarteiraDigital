using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Linq;

namespace CarteiraDigital.Servicos
{
    public class ConfiguracaoServico : IConfiguracaoServico
    {
        private const decimal PERCENTUAL_BONIFICACAO = 0.1m;
        private const decimal PERCENTUAL_TAXA = 0.01m;
        private const short TAMANHO_MAX_DESCRICAO = 50;

        private readonly IConfiguration configuracao;

        public ConfiguracaoServico(IConfiguration configuracao)
        {
            this.configuracao = configuracao;
        }

        public string[] ObterCulturasSuportadas()
        {
            return configuracao.GetSection("CulturasSuportadas").Get<string[]>();
        }

        public string ObterCulturaPadrao()
        {
            return configuracao.GetSection("CulturaPadrao").Value;
        }

        public byte ObterIdadeMinima()
        {
            var culturaAtual = CultureInfo.CurrentCulture.Name;
            var idadeMinimaSection = configuracao.GetSection("Usuario")
                                                 .GetSection("IdadeMinima");
            byte idadeMinima = 0;

            if (idadeMinimaSection.GetChildren().Any(x => x.Key.Equals(culturaAtual, StringComparison.InvariantCultureIgnoreCase)))
                idadeMinima = Convert.ToByte(idadeMinimaSection[culturaAtual]);

            return idadeMinima;
        }

        public short ObterTamanhoMaximoDescricao()
        {
            return TAMANHO_MAX_DESCRICAO;
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
