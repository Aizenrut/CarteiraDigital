using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public interface ILoginServico
    {
        Task<SignInResult> AutenticarAsync(CredenciaisDto credenciais);
        Task<bool> EhSenhaCorretaAsync(string usuario, string senha);
        TokenDto GerarToken(string nomeUsuario);
    }
}
