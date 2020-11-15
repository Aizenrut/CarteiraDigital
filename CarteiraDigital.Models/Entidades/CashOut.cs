namespace CarteiraDigital.Models
{
    public class CashOut : Operacao
    {
        public decimal ValorTaxa { get; set; }

        public CashOut()
        {
        }

        public CashOut(decimal valor, string descricao, decimal saldoAnterior, decimal valorTaxa) 
            : base(valor, descricao, saldoAnterior)
        {
            ValorTaxa = valorTaxa;
        }
    }
}
