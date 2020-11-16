namespace CarteiraDigital.Servicos
{
    public interface IConfiguracaoServico
    {
        byte ObterIdadeMinima();
        decimal ObterPercentualTaxa();
        decimal ObterPercentualBonificacao();
    }
}
