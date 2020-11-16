using System.ComponentModel;

namespace CarteiraDigital.Models.Enumeracoes
{
    public enum TipoOperacao
    {
        [Description("Cash-in")]
        CashIn,

        [Description("Cash-out")]
        CashOut,

        [Description("Transferência")]
        Transferencia
    }
}
