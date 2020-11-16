using System.IdentityModel.Tokens.Jwt;

namespace CarteiraDigital.Api.Servicos
{
    public class JwtServico : IJwtServico
    {
        public string ObterSubject(string token)
        {
            return new JwtSecurityToken(token).Subject;
        }
    }
}
