namespace CarteiraDigital.Models
{
    public class CashIn : Operacao
    {
        public decimal ValorBonificacao { get; set; }

        public CashIn() : base()
        {
        }

        public CashIn(int contaId, decimal valor, string descricao, decimal saldoAnterior, decimal valorBonificacao)
            : base(contaId, valor, descricao, saldoAnterior)
        {
            this.ValorBonificacao = valorBonificacao;
        }
    }
}
