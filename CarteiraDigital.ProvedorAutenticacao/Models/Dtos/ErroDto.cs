using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarteiraDigital.ProvedorAutenticacao.Models
{
    public class ErroDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ErroDto[] Details { get; set; }
        public ErroDto InnerError { get; set; }

        private ErroDto(string code, string message, ErroDto[] details, ErroDto innerError)
        {
            Code = code;
            Message = message;
            Details = details;
            InnerError = innerError;
        }

        public static ErroDto ParaNotFound(string busca)
        {
            return new ErroDto(code: StatusCodes.Status404NotFound.ToString(),
                               message: $"Not Found ({busca})",
                               details: null,
                               innerError: null);
        }

        public static ErroDto Para(Exception exception)
        {
            if (exception == null)
                return null;

            return new ErroDto(code: exception.HResult.ToString(),
                               message: exception.Message,
                               details: null,
                               innerError: Para(exception.InnerException));
        }

        public static ErroDto Para(ModelStateDictionary modelState)
        {
            if (modelState == null)
                return null;

            var details = modelState.Values.SelectMany(x => x.Errors)
                                           .Select(x => new ErroDto("400", x.ErrorMessage, null, null))
                                           .ToArray();

            return BadRequestError(details);
        }

        public static ErroDto Para(IEnumerable<IdentityError> identityErrors)
        {
            var details = identityErrors.Select(x => new ErroDto(x.Code, x.Description, null, null))
                                        .ToArray();

            return BadRequestError(details);
        }

        private static ErroDto BadRequestError(ErroDto[] details)
        {
            return new ErroDto(code: "400",
                               message: "O conteúdo enviado na requisição é inválido.",
                               details: details,
                               innerError: null);
        }
    }
}
