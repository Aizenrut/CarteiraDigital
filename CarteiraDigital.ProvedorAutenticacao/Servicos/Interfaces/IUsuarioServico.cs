using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public interface IUsuarioServico
    {
        Task<Usuario> ObterPeloNomeAsync(string nomeUsuario);
        bool EhUsuariovalido(Usuario usuario);
        Task<bool> EhUsuariovalidoAsync(string nomeUsuario);
        Task<IdentityResult> CadastrarAsync(CadastroUsuarioDto cadastro);
        Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha);
        Task<IdentityResult> InativarAsync(Usuario usuario);
    }
}
