using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using CarteiraDigital.ProvedorAutenticacao.Models;
using Microsoft.AspNetCore.Http;

namespace CarteiraDigital.ProvedorAutenticacao.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErroRespostaDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErroRespostaDto))]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioServico usuarioServico;

        public UsuariosController(IUsuarioServico usuarioServico)
        {
            this.usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Cadastra um novo usuário.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Cadastrar(CadastroUsuarioDto cadastro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var resultado = await usuarioServico.CadastrarAsync(cadastro);

            if (!resultado.Succeeded)
                return BadRequest(ErroRespostaDto.Para(resultado.Errors));

            var loginUri = Url.Action("Autenticar", "Login", null, HttpContext.Request.Scheme);

            return Created(loginUri, null);
        }

        /// <summary>
        /// Altera a senha de um usuário existente.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErroRespostaDto))]
        public async Task<IActionResult> AlterarSenha(AlteracaoSenhaDto alteracao)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErroRespostaDto.Para(ModelState));

            var usuario = await usuarioServico.ObterPeloNomeAsync(alteracao.NomeUsuario);

            if (!usuarioServico.EhUsuariovalido(usuario))
                return NotFound(ErroRespostaDto.ParaNotFound(alteracao.NomeUsuario));

            var resultado = await usuarioServico.AlterarSenhaAsync(usuario, alteracao.Senha, alteracao.NovaSenha);

            if (!resultado.Succeeded)
                return BadRequest(ErroRespostaDto.Para(resultado.Errors));

            return Ok();
        }

        /// <summary>
        /// Inativa um usuário.
        /// </summary>
        [HttpDelete("{nomeUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErroRespostaDto))]
        public async Task<IActionResult> Inativar(string nomeUsuario)
        {
            var usuario = await usuarioServico.ObterPeloNomeAsync(nomeUsuario);

            if (!usuarioServico.EhUsuariovalido(usuario))
                return NotFound(ErroRespostaDto.ParaNotFound(nomeUsuario));

            var resultado = await usuarioServico.InativarAsync(usuario);

            if (!resultado.Succeeded)
                return BadRequest(ErroRespostaDto.Para(resultado.Errors));

            return NoContent();
        }
    }
}
