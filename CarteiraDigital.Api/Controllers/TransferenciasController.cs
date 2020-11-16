using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CarteiraDigital.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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

        [HttpPost]
        public IActionResult GerarTransferencia(DadosOperacaoBinaria dados)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = Request.Headers[HeaderNames.Authorization];
            var contaOrigemId = requisicaoServico.ObterContaDoCliente(token);
            var contaDestinoId = contaServico.ObterIdPeloTitular(dados.UsuarioDestino);

            transferenciaServico.Efetivar(new OperacaoBinariaDto(contaOrigemId, contaDestinoId, dados.Valor, dados.Descricao));

            var movimentacaoUri = Url.Action("ConsultarExtrato", "Contas", null, HttpContext.Request.Scheme);
            return Created(movimentacaoUri, null);
        }
    }
}
