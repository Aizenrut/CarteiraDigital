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

        [HttpPost]
        public async Task<IActionResult> Autenticar(CredenciaisDto credenciais)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await usuarioServico.EhUsuariovalidoAsync(credenciais.NomeUsuario))
                return NotFound(); 

            var result = await loginServico.AutenticarAsync(credenciais);

            return await TratarSignInResult(result, credenciais);
        }

        private async Task<IActionResult> TratarSignInResult(SignInResult result, CredenciaisDto credenciais)
        {
            if (result.Succeeded)
                return Ok(loginServico.GerarToken(credenciais.NomeUsuario));

            if (result.IsNotAllowed)
                return StatusCode(StatusCodes.Status403Forbidden);

            if (result.IsLockedOut)
                return await TratarLockout(credenciais.NomeUsuario, credenciais.Senha);

            return Unauthorized();
        }

        private async Task<IActionResult> TratarLockout(string usuario, string senha)
        {
            var ehCorreta = await loginServico.EhSenhaCorretaAsync(usuario, senha);

            if (ehCorreta)
                return StatusCode(423);

            return Unauthorized();
        }
    }
}
