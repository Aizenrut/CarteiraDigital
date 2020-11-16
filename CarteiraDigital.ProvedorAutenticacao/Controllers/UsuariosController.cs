using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioServico usuarioServico;

        public UsuariosController(IUsuarioServico usuarioServico)
        {
            this.usuarioServico = usuarioServico;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CadastroUsuarioDto cadastro)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var resultado = await usuarioServico.CadastrarAsync(cadastro);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            var loginUri = Url.Action("Autenticar", "Login", null, HttpContext.Request.Scheme);

            return Created(loginUri, null);
        }

        [HttpPut]
        public async Task<IActionResult> AlterarSenha(AlteracaoSenhaDto alteracao)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioServico.ObterPeloNomeAsync(alteracao.NomeUsuario);

            if (!usuarioServico.EhUsuariovalido(usuario))
                return NotFound();

            var result = await usuarioServico.AlterarSenhaAsync(usuario, alteracao.Senha, alteracao.NovaSenha);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpDelete("{nomeUsuario}")]
        public async Task<IActionResult> Inativar(string nomeUsuario)
        {
            var usuario = await usuarioServico.ObterPeloNomeAsync(nomeUsuario);

            if (!usuarioServico.EhUsuariovalido(usuario))
                return NotFound();

            var result = await usuarioServico.InativarAsync(usuario);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
