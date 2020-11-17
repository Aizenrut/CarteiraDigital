using CarteiraDigital.Api.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarteiraDigital.Api.Testes.Servicos
{
    [TestClass]
    public class JwtServicoTestes
    {
        private const string TOKEN_TESTE = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0ZSIsIm5hbWUiOiJ0ZXN0ZSB1bml0YXJpbyIsImlhdCI6MTUxNjIzOTAyMn0.BNoFJlDymCIHgJJfSVHzNm7hfTw5FarL5J-8H0rMekM";

        [TestMethod]
        public void FormatarToken_TokenComBearerInicialMaiuscula_DeveRetornarOTokenSemBearer()
        {
            // Arrange
            var jwtServico = new JwtServico();

            // Act
            var resultado = jwtServico.FormatarToken("Bearer token123");

            // Assert
            Assert.AreEqual("token123", resultado);
        }

        [TestMethod]
        public void FormatarToken_TokenComBearerMinusculo_DeveRetornarOTokenSemBearer()
        {
            // Arrange
            var jwtServico = new JwtServico();

            // Act
            var resultado = jwtServico.FormatarToken("bearer token123");

            // Assert
            Assert.AreEqual("token123", resultado);
        }

        [TestMethod]
        public void FormatarToken_TokenComBearerMaiusculo_DeveRetornarOTokenSemBearer()
        {
            // Arrange
            var jwtServico = new JwtServico();

            // Act
            var resultado = jwtServico.FormatarToken("BEARER token123");

            // Assert
            Assert.AreEqual("token123", resultado);
        }

        [TestMethod]
        public void FormatarToken_TokenComBearerEMuitosEspacos_DeveRetornarOTokenSemBearer()
        {
            // Arrange
            var jwtServico = new JwtServico();

            // Act
            var resultado = jwtServico.FormatarToken(" Bearer    token123  ");

            // Assert
            Assert.AreEqual("token123", resultado);
        }

        [TestMethod]
        public void ObterSubject_TokenComBearerEMuitosEspacos_DeveRetornarOTokenSemBearer()
        {
            // Arrange
            var jwtServico = new JwtServico();

            // Act
            var resultado = jwtServico.ObterSubject(TOKEN_TESTE);

            // Assert
            Assert.AreEqual("teste", resultado);
        }
    }
}
