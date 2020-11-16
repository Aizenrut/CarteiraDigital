using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace CarteiraDigital.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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

        [HttpGet("saldo")]
        public IActionResult ConsultarSaldoDisponivel()
        {
            var contaId = ObterContaPeloToken();
            var saldo = contaServico.ObterSaldoAtual(contaId);

            return Ok((SaldoDto)saldo.ParaMoeda());
        }

        [HttpGet("extrato")]
        public IActionResult ConsultarExtrato([FromQuery] DateTime dataInicial,
                                              [FromQuery] DateTime dataFinal)
        {
            if (!ModelState.IsValid)
                return BadRequest();

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
