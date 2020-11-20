using CarteiraDigital.Api.Models;
using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace CarteiraDigital.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroRespostaDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroRespostaDto))]
    public class CashOutsController : ControllerBase
    {
        private readonly ICashOutServico cashOutServico;
        private readonly IRequisicaoServico requisicaoServico;

        public CashOutsController(ICashOutServico cashOutServico,
                                  IRequisicaoServico requisicaoServico)
        {
            this.cashOutServico = cashOutServico;
            this.requisicaoServico = requisicaoServico;
        }

        /// <summary>
        /// Gera uma operação de cash-out.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GerarCashOut(DadosOperacaoUnaria dados)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var token = Request.Headers[HeaderNames.Authorization];
            var contaId = requisicaoServico.ObterContaDoCliente(token);

            await cashOutServico.Gerar(new OperacaoUnariaDto(contaId, dados.Valor, dados.Descricao));

            var movimentacaoUri = Url.Action("ConsultarExtrato", "Contas", null, HttpContext.Request.Scheme);
            return Created(movimentacaoUri, null);
        }
    }
}
