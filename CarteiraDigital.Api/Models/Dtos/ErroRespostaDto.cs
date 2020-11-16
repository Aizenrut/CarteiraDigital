using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace CarteiraDigital.Api.Models
{
    public class ErroRespostaDto
    {
        public ErroDto Error { get; set; }

        private ErroRespostaDto(ErroDto error)
        {
            Error = error;
        }

        public static ErroRespostaDto ParaNotFound(string busca)
        {
            return new ErroRespostaDto(ErroDto.ParaNotFound(busca));
        }

        public static ErroRespostaDto Para(Exception exception)
        {
            return new ErroRespostaDto(ErroDto.Para(exception));
        }

        public static ErroRespostaDto Para(ModelStateDictionary modelState)
        {
            return new ErroRespostaDto(ErroDto.Para(modelState));
        }
    }
}
