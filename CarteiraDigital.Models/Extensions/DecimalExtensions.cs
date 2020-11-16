using System.Globalization;

namespace CarteiraDigital.Models
{
    public static class DecimalExtensions
    {
        public static string ParaMoeda(this decimal valor)
        {
            return valor.ToString("C", CultureInfo.CurrentCulture);
        }
    }
}
