namespace CarteiraDigital.Models
{
    public class CashOut : Operacao
    {
        public decimal ValorTaxa { get; set; }

        public CashOut()
        {
        }

        public CashOut(int contaId, decimal valor, string descricao, decimal saldoAnterior, decimal valorTaxa) 
            : base(contaId, valor, descricao, saldoAnterior)
        {
            ValorTaxa = valorTaxa;
        }
    }
}
