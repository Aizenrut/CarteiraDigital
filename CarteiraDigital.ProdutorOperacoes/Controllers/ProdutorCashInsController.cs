using CarteiraDigital.Models;
using CarteiraDigital.ProdutorOperacoes.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarteiraDigital.ProdutorOperacoes.Controllers
{
    public class ProdutorCashInsController : ControllerBase
    {
        private readonly IProdutorOperacoesServico<EfetivarOperacaoUnariaDto> produtor;
        private readonly ILogger<ProdutorCashInsController> logger;

        private const string FILA_CASHINS = "cashIns";

        public ProdutorCashInsController(IProdutorOperacoesServico<EfetivarOperacaoUnariaDto> produtor,
                                         ILogger<ProdutorCashInsController> logger)
        {
            this.produtor = produtor;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Produzir(EfetivarOperacaoUnariaDto cashIn)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            produtor.Produzir(cashIn, FILA_CASHINS);
            logger.LogInformation($"Cash-in {cashIn.OperacaoId} enfileirado.");

            return Ok();
        }
    }
}
