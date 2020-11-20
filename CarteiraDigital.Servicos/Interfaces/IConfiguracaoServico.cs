using System.Globalization;

namespace CarteiraDigital.Servicos
{
    public interface IConfiguracaoServico
    {
        string[] ObterCulturasSuportadas();
        string ObterCulturaPadrao();
        byte ObterIdadeMinima();
        decimal ObterPercentualTaxa();
        decimal ObterPercentualBonificacao();
    }
}
