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
    public class TransferenciasController : ControllerBase
    {
        private readonly ITransferenciaServico transferenciaServico;
        private readonly IRequisicaoServico requisicaoServico;
        private readonly IContaServico contaServico;

        public TransferenciasController(ITransferenciaServico transferenciaServico,
                                        IRequisicaoServico requisicaoServico,
                                        IContaServico contaServico)
        {
            this.transferenciaServico = transferenciaServico;
            this.requisicaoServico = requisicaoServico;
            this.contaServico = contaServico;
        }

        /// <summary>
        /// Gera uma operação de transferência para um determinado usuário.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GerarTransferencia(DadosOperacaoBinaria dados)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var token = Request.Headers[HeaderNames.Authorization];
            var contaOrigemId = requisicaoServico.ObterContaDoCliente(token);
            var contaDestinoId = contaServico.ObterIdPeloTitular(dados.UsuarioDestino);

            await transferenciaServico.Gerar(new OperacaoBinariaDto(contaOrigemId, contaDestinoId, dados.Valor, dados.Descricao));

            var movimentacaoUri = Url.Action("ConsultarExtrato", "Contas", null, HttpContext.Request.Scheme);
            return Created(movimentacaoUri, null);
        }
    }
}
