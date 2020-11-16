using CarteiraDigital.Api.Models;
using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace CarteiraDigital.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroRespostaDto))]
    public class ContasController : ControllerBase
    {
        private readonly IContaServico contaServico;
        private readonly IRequisicaoServico requisicaoServico;

        public ContasController(IContaServico contaServico,
                                IRequisicaoServico requisicaoServico)
        {
            this.contaServico = contaServico;
            this.requisicaoServico = requisicaoServico;
        }

        /// <summary>
        /// Consulta o saldo atual da conta.
        /// </summary>
        [HttpGet("saldo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaldoDto))]
        public IActionResult ConsultarSaldoDisponivel()
        {
            var contaId = ObterContaPeloToken();
            var saldo = contaServico.ObterSaldoAtual(contaId);

            return Ok((SaldoDto)saldo.ParaMoeda());
        }

        /// <summary>
        /// Consulta o extrato das movimentações da conta.
        /// </summary>
        [HttpGet("extrato")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExtratoDto))]
        public IActionResult ConsultarExtrato([FromQuery] DateTime dataInicial,
                                              [FromQuery] DateTime dataFinal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var contaId = ObterContaPeloToken();
            var movimentacao = contaServico.ObterMovimentacao(contaId, dataInicial, dataFinal);

            return Ok((ExtratoDto)movimentacao);
        }

        private int ObterContaPeloToken()
        {
            var token = Request.Headers[HeaderNames.Authorization];
            return requisicaoServico.ObterContaDoCliente(token);
        }
    }
}
