namespace CarteiraDigital.Models
{
    public class Transferencia : Operacao
    {
        public TipoTransferencia TipoTransferencia { get; set; }

        public Transferencia()
        {
        }

        public Transferencia(decimal valor, string descricao, decimal saldoAnterior, TipoTransferencia tipoTransferencia)
            : base(valor, descricao, saldoAnterior)
        {
            TipoTransferencia = tipoTransferencia;
        }
    }
}
