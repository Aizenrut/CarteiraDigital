using CarteiraDigital.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CarteiraDigital.Api.Models
{
    public class ErroRespostaFilter : IExceptionFilter
    {
        private readonly ILogger logger;

        public ErroRespostaFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ErroRespostaFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            if (!(context.Exception is CarteiraDigitalException))
            {
                statusCode = StatusCodes.Status500InternalServerError;
                logger.LogError($"Erro: { context.Exception.Message }", context.Exception);
            }

            context.Result = new ObjectResult(ErroRespostaDto.Para(context.Exception)) { StatusCode = statusCode };
            context.ExceptionHandled = true;
        }
    }
}
