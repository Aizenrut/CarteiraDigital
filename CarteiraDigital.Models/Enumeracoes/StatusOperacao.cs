using System.ComponentModel;

namespace CarteiraDigital.Models
{
    public enum StatusOperacao
    {
        [Description("Pendente")]
        Pendente,

        [Description("Efetivada")]
        Efetivada,

        [Description("Com erro")]
        ComErro
    }
}
