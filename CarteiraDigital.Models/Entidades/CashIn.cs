namespace CarteiraDigital.Models
{
    public class CashIn : Operacao
    {
        public CashIn() : base()
        {
        }

        public CashIn(int contaId, decimal valor, string descricao, decimal saldoAnterior)
            : base(contaId, valor, descricao, saldoAnterior)
        {
        }
    }
}
