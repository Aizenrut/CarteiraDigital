using CarteiraDigital.Models;
using CarteiraDigital.ProdutorOperacoes.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarteiraDigital.ProdutorOperacoes.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutorCashOutsController : ControllerBase
    {
        private readonly IProdutorOperacoesServico<EfetivarOperacaoUnariaDto> produtor;
        private readonly ILogger<ProdutorCashOutsController> logger;

        private const string FILA_CASHOUTS = "cashOuts";

        public ProdutorCashOutsController(IProdutorOperacoesServico<EfetivarOperacaoUnariaDto> produtor,
                                          ILogger<ProdutorCashOutsController> logger)
        {
            this.produtor = produtor;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Produzir(EfetivarOperacaoUnariaDto cashOut)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            produtor.Produzir(cashOut, FILA_CASHOUTS);
            logger.LogInformation($"Cash-out {cashOut.OperacaoId} enfileirado.");

            return Ok();
        }
    }
}
