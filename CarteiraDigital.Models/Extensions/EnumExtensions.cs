using System;
using System.ComponentModel;

namespace CarteiraDigital.Models
{
    public static class EnumExtensions
    {
        public static string ObterDescricao<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var descricao = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var atributos = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (atributos != null && atributos.Length > 0)
                {
                    descricao = ((DescriptionAttribute)atributos[0]).Description;
                }
            }

            return descricao;
        }
    }
}
