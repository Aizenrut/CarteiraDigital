using CarteiraDigital.ProvedorAutenticacao.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CarteiraDigital.ProvedorAutenticacao.Testes.Servicos
{
    [TestClass]
    public class LoginServicoTestes
    {
        [TestMethod]
        public void ObterClaims_UsuarioTeste_DeveRetornarSubComONomeDoUsuario()
        {
            // Arrange
            var nomeUsuario = "teste";
            var loginServico = new LoginServico(null, null, null, null);

            // Act
            var result = loginServico.ObterClaims(nomeUsuario).ToList();

            // Assert
            Assert.IsTrue(result.Any(x => x.Type == JwtRegisteredClaimNames.Sub && x.Value == nomeUsuario));
            Assert.IsTrue(result.Any(x => x.Type == JwtRegisteredClaimNames.Iat));
            Assert.IsTrue(result.Any(x => x.Type == JwtRegisteredClaimNames.Jti));
        }
    }
}
