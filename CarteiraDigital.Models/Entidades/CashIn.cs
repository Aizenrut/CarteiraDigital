using System;

namespace CarteiraDigital.Models
{
    public class CashIn : Operacao
    {
        public CashIn() : base()
        {
        }

        public CashIn(decimal valor, string descricao, decimal saldoAnterior)
            : base(valor, descricao, saldoAnterior)
        {
        }
    }
}
