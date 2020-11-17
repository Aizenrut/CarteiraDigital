namespace CarteiraDigital.ProdutorOperacoes.Servicos
{
    public interface IProdutorOperacoesServico<T> where T : struct
    {
        void Produzir(T operacao, string routingKey);
    }
}
