using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CarteiraDigital.ProvedorAutenticacao.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroRespostaDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroRespostaDto))]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServico loginServico;
        private readonly IUsuarioServico usuarioServico;

        public LoginController(ILoginServico loginServico,
                               IUsuarioServico usuarioServico)
        {
            this.loginServico = loginServico;
            this.usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Realiza a autenticação e geração do token.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Autenticar(CredenciaisDto credenciais)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            if (!await usuarioServico.EhUsuariovalidoAsync(credenciais.NomeUsuario))
                return NotFound(ErroRespostaDto.ParaNotFound(credenciais.NomeUsuario)); 

            var result = await loginServico.AutenticarAsync(credenciais);

            return await TratarSignInResult(result, credenciais);
        }

        private async Task<IActionResult> TratarSignInResult(SignInResult result, CredenciaisDto credenciais)
        {
            if (result.Succeeded)
                return Ok(loginServico.GerarToken(credenciais.NomeUsuario));

            if (result.IsLockedOut)
                return await TratarLockout(credenciais.NomeUsuario, credenciais.Senha);

            return Unauthorized();
        }

        private async Task<IActionResult> TratarLockout(string usuario, string senha)
        {
            var ehCorreta = await loginServico.EhSenhaCorretaAsync(usuario, senha);

            if (ehCorreta)
                return StatusCode(StatusCodes.Status423Locked);

            return Unauthorized();
        }
    }
}
