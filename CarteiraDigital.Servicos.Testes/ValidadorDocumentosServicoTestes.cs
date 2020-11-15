using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class ValidadorDocumentosServicoTestes
    {
        [TestMethod]
        public void EhCpfValido_CpfVazio_DeveRetornarFalse()
        {
            // Arrange
            var validadorDocumentos = new ValidacaoDocumentosServico();

            // Act
            var resultado = validadorDocumentos.EhCpfValido(string.Empty);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhCpfValido_CpfComMenosQue11Digitos_DeveRetornarFalse()
        {
            // Arrange
            var validadorDocumentos = new ValidacaoDocumentosServico();

            // Act
            var resultado = validadorDocumentos.EhCpfValido("240.144.170");

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhCpfValido_CpfInvalido_DeveRetornarFalse()
        {
            // Arrange
            var validadorDocumentos = new ValidacaoDocumentosServico();

            // Act
            var resultado = validadorDocumentos.EhCpfValido("123.456.789-10");

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhCpfValido_CpfValido_DeveRetornarTrue()
        {
            // Arrange
            var validadorDocumentos = new ValidacaoDocumentosServico();

            // Act
            var resultado = validadorDocumentos.EhCpfValido("240.144.170-00");

            // Assert
            Assert.IsTrue(resultado);
        }
    }
}
