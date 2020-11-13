using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly UserManager<Usuario> userManager;

        public UsuarioServico(UserManager<Usuario> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Usuario> ObterPeloNomeAsync(string nomeUsuario)
        {
            return await userManager.FindByNameAsync(nomeUsuario);
        }

        public bool EhUsuariovalido(Usuario usuario)
        {
            return usuario != null && usuario.Ativo;
        }

        public async Task<bool> EhUsuariovalidoAsync(string nomeUsuario)
        {
            var usuario = await ObterPeloNomeAsync(nomeUsuario);

            return EhUsuariovalido(usuario);
        }

        public async Task<IdentityResult> CadastrarAsync(CadastroUsuarioDto cadastro)
        {
            var usuario = new Usuario(cadastro.NomeUsuario);

            return await userManager.CreateAsync(usuario, cadastro.Senha);
        }        

        public async Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha)
        {
            return await userManager.ChangePasswordAsync(usuario, senhaAtual, novaSenha);
        }

        public async Task<IdentityResult> InativarAsync(Usuario usuario)
        {
            usuario.Ativo = false;

            return await userManager.UpdateAsync(usuario);
        }
    }
}
