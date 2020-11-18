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

        /// <summary>
        /// Gera uma operação de cash-in.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GerarCashIn(DadosOperacaoUnaria dados)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var token = Request.Headers[HeaderNames.Authorization];
            var contaId = requisicaoServico.ObterContaDoCliente(token);

            await cashInServico.Gerar(new OperacaoUnariaDto(contaId, dados.Valor, dados.Descricao));

            var movimentacaoUri = Url.Action("ConsultarExtrato", "Contas", null, HttpContext.Request.Scheme);
            return Created(movimentacaoUri, null);
        }
    }
}
