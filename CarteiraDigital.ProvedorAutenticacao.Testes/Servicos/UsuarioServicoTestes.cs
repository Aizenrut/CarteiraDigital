using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using CarteiraDigital.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace CarteiraDigital.ProvedorAutenticacao.Testes.Servicos
{
    [TestClass]
    public class UsuarioServicoTestes
    {
        [TestMethod]
        public void EhUsuarioValido_PassandoNulo_DeveRetornarFalse()
        {
            // Arrange
            var usuarioServico = new UsuarioServico(null, null, null, null, null);

            // Act
            var resultado = usuarioServico.EhUsuariovalido(null);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhUsuarioValido_UsuarioInativo_DeveRetornarFalse()
        {
            // Arrange
            var usuarioServico = new UsuarioServico(null, null, null, null, null);

            // Act
            var resultado = usuarioServico.EhUsuariovalido(new Usuario());

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhUsuarioValido_UsuarioAtivo_DeveRetornarFalse()
        {
            // Arrange
            var usuarioServico = new UsuarioServico(null, null, null, null, null);

            // Act
            var resultado = usuarioServico.EhUsuariovalido(new Usuario() { Ativo = true });

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ObterIdadeMinima_RegiaoSemIdadeCadastrada_DeveRetornarZero()
        {
            // Arrange
            var configurationSection = Substitute.For<IConfigurationSection>();
            configurationSection.GetSection("IdadeMinima").Returns(configurationSection);
            configurationSection.GetChildren().Returns(new List<IConfigurationSection> { configurationSection });

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetSection("Usuarios").Returns(configurationSection);

            var usuarioServico = new UsuarioServico(null, null, null, null, configurationSection);

            // Act
            var resultado = usuarioServico.ObterIdadeMinima();

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void PossuiIdadeMinima_RegiaoSemIdadeCadastrada_DeveRetornarTrue()
        {
            // Arrange
            var configurationSection = Substitute.For<IConfigurationSection>();
            configurationSection.GetSection("IdadeMinima").Returns(configurationSection);
            configurationSection.GetChildren().Returns(new List<IConfigurationSection> { configurationSection });

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetSection("Usuarios").Returns(configurationSection);

            var usuarioServico = new UsuarioServico(null, null, null, null, configurationSection);

            // Act
            var resultado = usuarioServico.PossuiIdadeMinima(new Usuario());

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void ValidarUsuario_CpfInvalido_DeveLancarExcecao()
        {
            // Arrange
            var validadorDocumentos = Substitute.For<IValidacaoDocumentosServico>();
            validadorDocumentos.EhCpfValido(Arg.Any<string>()).Returns(false);

            var usuarioServico = new UsuarioServico(null, null, null, validadorDocumentos, null);

            // Act
            Action acao = () => usuarioServico.ValidarUsuario(new Usuario());

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O CPF informado é inválido!"));
        }

        [TestMethod]
        public void ValidarUsuario_CpfValidoSemIdadeMinima_NaoDeveLancarExcecao()
        {
            // Arrange
            var validadorDocumentos = Substitute.For<IValidacaoDocumentosServico>();
            validadorDocumentos.EhCpfValido(Arg.Any<string>()).Returns(true);

            var configurationSection = Substitute.For<IConfigurationSection>();
            configurationSection.GetSection("IdadeMinima").Returns(configurationSection);
            configurationSection.GetChildren().Returns(new List<IConfigurationSection> { configurationSection });

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetSection("Usuarios").Returns(configurationSection);

            var usuarioServico = new UsuarioServico(null, null, null, validadorDocumentos, configuration);

            // Act and Assert
            usuarioServico.ValidarUsuario(new Usuario());
        }
    }
}
