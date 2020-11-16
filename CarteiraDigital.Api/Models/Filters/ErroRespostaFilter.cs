using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CarteiraDigital.Api.Models
{
    public class ErroRespostaFilter : IExceptionFilter
    {
        private readonly ILogger logger;
        private readonly ILogServico logServico;

        public ErroRespostaFilter(ILoggerFactory loggerFactory,
                                  ILogServico logServico)
        {
            logger = loggerFactory.CreateLogger<ErroRespostaFilter>();
            this.logServico = logServico;
        }

        public void OnException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            if (!(context.Exception is CarteiraDigitalException))
            {
                statusCode = StatusCodes.Status500InternalServerError;
                logServico.Log(context.Exception.Message, context.Exception);
                logger.LogWarning(context.Exception.Message, context.Exception);
            }

            context.Result = new ObjectResult(ErroRespostaDto.Para(context.Exception)) { StatusCode = statusCode };
            context.ExceptionHandled = true;
        }
    }
}
