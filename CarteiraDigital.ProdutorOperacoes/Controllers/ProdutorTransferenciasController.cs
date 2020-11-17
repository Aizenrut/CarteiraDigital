using CarteiraDigital.Models;
using CarteiraDigital.ProdutorOperacoes.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarteiraDigital.ProdutorOperacoes.Controllers
{
    public class ProdutorTransferenciasController : ControllerBase
    {
        private readonly IProdutorOperacoesServico<EfetivarOperacaoBinariaDto> produtor;
        private readonly ILogger<ProdutorTransferenciasController> logger;

        private const string FILA_TRANSFERENCIAS = "transferencias";

        public ProdutorTransferenciasController(IProdutorOperacoesServico<EfetivarOperacaoBinariaDto> produtor,
                                                ILogger<ProdutorTransferenciasController> logger)
        {
            this.produtor = produtor;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Produzir(EfetivarOperacaoBinariaDto transferencia)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            produtor.Produzir(transferencia, FILA_TRANSFERENCIAS);
            logger.LogInformation($"Transferências {transferencia.OperacaoEntradaId} e {transferencia.OperacaoSaidaId} enfileiradas.");

            return Ok();
        }
    }
}
