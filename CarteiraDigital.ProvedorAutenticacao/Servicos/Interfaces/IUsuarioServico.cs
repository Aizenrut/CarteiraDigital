using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public interface IUsuarioServico
    {
        Task<Usuario> ObterPeloNomeAsync(string nomeUsuario);
        bool EhUsuarioValido(Usuario usuario);
        Task<bool> EhUsuarioValidoAsync(string nomeUsuario);
        Task<IdentityResult> CadastrarAsync(CadastroUsuarioDto cadastro);
        Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha);
        bool PossuiIdadeMinima(Usuario usuario);
        void ValidarUsuario(Usuario usuario);
    }
}
