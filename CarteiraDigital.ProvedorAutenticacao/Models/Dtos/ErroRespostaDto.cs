using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace CarteiraDigital.ProvedorAutenticacao.Models
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

        public static ErroRespostaDto Para(IEnumerable<IdentityError> identityErrors)
        {
            return new ErroRespostaDto(ErroDto.Para(identityErrors));
        }
    }
}
