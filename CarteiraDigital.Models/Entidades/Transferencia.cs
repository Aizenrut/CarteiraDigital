namespace CarteiraDigital.Models
{
    public class Transferencia : Operacao
    {
        public TipoMovimentacao TipoMovimentacao { get; set; }

        public Transferencia()
        {
        }

        public Transferencia(decimal valor, string descricao, decimal saldoAnterior, TipoMovimentacao tipoMovimentacao)
            : base(valor, descricao, saldoAnterior)
        {
            TipoMovimentacao = tipoMovimentacao;
        }
    }
}
