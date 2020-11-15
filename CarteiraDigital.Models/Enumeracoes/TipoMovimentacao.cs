using System.ComponentModel;

namespace CarteiraDigital.Models
{
    public enum TipoMovimentacao
    {
        [Description("Saída")]
        Saida,

        [Description("Entrada")]
        Entrada
    }
}
