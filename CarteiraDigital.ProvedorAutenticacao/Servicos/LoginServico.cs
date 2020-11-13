using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Servicos
{
    public class LoginServico : ILoginServico
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public LoginServico(IConfiguration configuration,
                            UserManager<Usuario> userManager,
                            SignInManager<Usuario> signInManager,
                            JwtSecurityTokenHandler tokenHandler)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenHandler = tokenHandler;
        }

        public async Task<SignInResult> AutenticarAsync(CredenciaisDto credenciais)
        {
            return await signInManager.PasswordSignInAsync(credenciais.NomeUsuario, credenciais.Senha, false, true);
        }

        public async Task<bool> EhSenhaCorretaAsync(string nomeUsuario, string senha)
        {
            var usuario = await userManager.FindByNameAsync(nomeUsuario);

            return await userManager.CheckPasswordAsync(usuario, senha);
        }

        public TokenDto GerarToken(string nomeUsuario)
        {
            var claimsSection = configuration.GetSection("Claims");

            var dataEmissao = DateTime.Now;
            var dataExpiracao = dataEmissao.AddMinutes(30);

            var token = new JwtSecurityToken(issuer: claimsSection["Iss"],
                                             audience: claimsSection["Aud"],
                                             claims: ObterClaims(nomeUsuario),
                                             notBefore: dataEmissao,
                                             expires: dataExpiracao,
                                             signingCredentials: ObterSigningCredentials());

            var tokenString = tokenHandler.WriteToken(token);

            return new TokenDto(nomeUsuario, tokenString, dataEmissao, dataExpiracao);
        }

        public Claim[] ObterClaims(string nomeUsuario)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nomeUsuario),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
        }

        public SigningCredentials ObterSigningCredentials()
        {
            var keysSection = configuration.GetSection("Keys");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keysSection["JwtBearer"]));
            
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}
