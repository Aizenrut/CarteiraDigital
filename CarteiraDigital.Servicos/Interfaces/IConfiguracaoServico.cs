using System.Globalization;

namespace CarteiraDigital.Servicos
{
    public interface IConfiguracaoServico
    {
        string[] ObterCulturasSuportadas();
        string ObterCulturaPadrao();
        byte ObterIdadeMinima();
        short ObterTamanhoMaximoDescricao();
        decimal ObterPercentualTaxa();
        decimal ObterPercentualBonificacao();
    }
}
