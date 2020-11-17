namespace CarteiraDigital.Models
{
    public struct EfetivarOperacaoBinariaDto
    {
        public int OperacaoSaidaId { get; set; }
        public int OperacaoEntradaId { get; set; }

        public EfetivarOperacaoBinariaDto(int operacaoSaidaId, int operacaoEntradaId)
        {
            OperacaoSaidaId = operacaoSaidaId;
            OperacaoEntradaId = operacaoEntradaId;
        }
    }
}
