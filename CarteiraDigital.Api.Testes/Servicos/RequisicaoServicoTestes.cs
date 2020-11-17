using CarteiraDigital.Api.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CarteiraDigital.Api.Testes.Servicos
{
    [TestClass]
    public class RequisicaoServicoTestes
    {
        [TestMethod]
        public void ObterContaDoCliente_PassandoJwtComBearer_DeveRetornarId()
        {
            // Arrange
            var conta = new Conta() 
            {
                Id = 1,
                UsuarioTitular = "teste"
            };
            var token = "token123";

            var jwtServico = Substitute.For<IJwtServico>();
            jwtServico.ObterSubject(token).Returns(conta.UsuarioTitular);

            var contaServico = Substitute.For<IContaServico>();
            contaServico.ObterIdPeloTitular(conta.UsuarioTitular).Returns(conta.Id);

            var requisicaoServico = new RequisicaoServico(contaServico, jwtServico);

            // Act
            var resultado = requisicaoServico.ObterContaDoCliente(token);

            // Assert
            jwtServico.Received(1).ObterSubject(token);
            contaServico.Received(1).ObterIdPeloTitular(conta.UsuarioTitular);
            Assert.AreEqual(conta.Id, resultado);
        }
    }
}
