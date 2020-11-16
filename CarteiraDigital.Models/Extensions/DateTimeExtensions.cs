using System;
using System.Globalization;

namespace CarteiraDigital.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ParaData(this DateTime data)
        {
            return data.ToString("dddd, dd MMMM yyyy HH:mm:ss", CultureInfo.CurrentCulture);
        }
    }
}
