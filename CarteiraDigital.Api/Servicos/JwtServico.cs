using System;
using System.IdentityModel.Tokens.Jwt;

namespace CarteiraDigital.Api.Servicos
{
    public class JwtServico : IJwtServico
    {
        public string ObterSubject(string token)
        {
            return new JwtSecurityToken(FormatarToken(token)).Subject;
        }

        public string FormatarToken(string token)
        {
            return token.Replace("Bearer", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
        }
    }
}
