namespace CarteiraDigital.Models
{
    public class Transferencia : Operacao
    {
        public TipoMovimentacao TipoMovimentacao { get; set; }

        public Transferencia()
        {
        }

        public Transferencia(int contaId, decimal valor, string descricao, decimal saldoAnterior, TipoMovimentacao tipoMovimentacao)
            : base(contaId, valor, descricao, saldoAnterior)
        {
            TipoMovimentacao = tipoMovimentacao;
        }
    }
}
