using System.ComponentModel;

namespace CarteiraDigital.Models
{
    public enum TipoTransferencia
    {
        [Description("Saída")]
        Saida,

        [Description("Entrada")]
        Entrada
    }
}
