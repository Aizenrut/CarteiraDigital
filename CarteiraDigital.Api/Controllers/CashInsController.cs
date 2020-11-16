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
    [Route("api/[controller]")]
    public class CashInsController : ControllerBase
    {
        private readonly ICashInServico cashInServico;
        private readonly IRequisicaoServico requisicaoServico;

        public CashInsController(ICashInServico cashInServico,
                                 IRequisicaoServico requisicaoServico)
        {
            this.cashInServico = cashInServico;
            this.requisicaoServico = requisicaoServico;
        }

        [HttpPost]
        public IActionResult GerarCashIn(DadosOperacaoUnaria dados)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = Request.Headers[HeaderNames.Authorization];
            var contaId = requisicaoServico.ObterContaDoCliente(token);

            cashInServico.Efetivar(new OperacaoUnariaDto(contaId, dados.Valor, dados.Descricao));

            var movimentacaoUri = Url.Action("ConsultarExtrato", "Contas", null, HttpContext.Request.Scheme);
            return Created(movimentacaoUri, null);
        }
    }
}
