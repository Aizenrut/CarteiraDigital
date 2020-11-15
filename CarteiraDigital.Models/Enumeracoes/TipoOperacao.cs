using System.ComponentModel;

namespace CarteiraDigital.Models.Enumeracoes
{
    public enum TipoOperacao
    {
        [Description("CashIn")]
        CashIn,

        [Description("CashOut")]
        CashOut,

        [Description("Transferência")]
        Transferencia
    }
}
