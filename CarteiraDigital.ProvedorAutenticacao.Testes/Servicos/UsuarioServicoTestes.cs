using CarteiraDigital.Models;
using CarteiraDigital.ProvedorAutenticacao.Models;
using CarteiraDigital.ProvedorAutenticacao.Servicos;
using CarteiraDigital.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

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
            var resultado = usuarioServico.EhUsuarioValido(null);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhUsuarioValido_UsuarioValido_DeveRetornarTrue()
        {
            // Arrange
            var usuarioServico = new UsuarioServico(null, null, null, null, null);

            // Act
            var resultado = usuarioServico.EhUsuarioValido(new Usuario());

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PossuiIdadeMinima_RegiaoSemIdadeCadastrada_DeveRetornarTrue()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();

            var usuarioServico = new UsuarioServico(null, null, null, null, configuracaoServico);

            // Act
            var resultado = usuarioServico.PossuiIdadeMinima(new Usuario());

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PossuiIdadeMinima_RegiaoComIdadeCadastrada_DeveRetornarFalse()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns((byte)18);

            var usuarioServico = new UsuarioServico(null, null, null, null, configuracaoServico);

            // Act
            var resultado = usuarioServico.PossuiIdadeMinima(new Usuario() { DataNascimento = DateTime.Now.AddDays(-365) });

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void PossuiIdadeMinima_UsuarioComIdadeIgualAMinima_DeveRetornarTrue()
        {
            // Arrange
            byte idade = 18;

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns(idade);

            var usuarioServico = new UsuarioServico(null, null, null, null, configuracaoServico);

            // Act
            var resultado = usuarioServico.PossuiIdadeMinima(new Usuario() { DataNascimento = DateTime.Now.AddDays(-idade * 365) });

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PossuiIdadeMinima_UsuarioComIdadeSuperiorAMinima_DeveRetornarTrue()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns((byte)18);

            var usuarioServico = new UsuarioServico(null, null, null, null, configuracaoServico);

            // Act
            var resultado = usuarioServico.PossuiIdadeMinima(new Usuario() { DataNascimento = DateTime.Now.AddDays(-20 * 365) });

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
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O CPF informado é inválido!"));
        }

        [TestMethod]
        public void ValidarUsuario_CpfValidoEIdadeInferiorAMinima_DeveLancarExcecao()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns((byte)18);

            var validadorDocumentos = Substitute.For<IValidacaoDocumentosServico>();
            validadorDocumentos.EhCpfValido(Arg.Any<string>()).Returns(true);

            var usuarioServico = new UsuarioServico(null, null, null, validadorDocumentos, configuracaoServico);

            // Act
            Action acao = () => usuarioServico.ValidarUsuario(new Usuario() { DataNascimento = DateTime.Now.AddDays(-365) });

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("Não possui a idade mínima para cadastro (18 anos)!"));
        }

        [TestMethod]
        public void ValidarUsuario_CpfValidoEIdadeIgualAMinima_DevePermitir()
        {
            // Arrange
            byte idade = 18;

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns(idade);

            var validadorDocumentos = Substitute.For<IValidacaoDocumentosServico>();
            validadorDocumentos.EhCpfValido(Arg.Any<string>()).Returns(true);

            var usuarioServico = new UsuarioServico(null, null, null, validadorDocumentos, configuracaoServico);

            // Act and Assert
            usuarioServico.ValidarUsuario(new Usuario() { DataNascimento = DateTime.Now.AddDays(-idade * 365) });
        }

        [TestMethod]
        public void ValidarUsuario_CpfValidoEIdadeSuperiorAMinima_DevePermitir()
        {
            // Arrange
            byte idade = 18;

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterIdadeMinima().Returns(idade);

            var validadorDocumentos = Substitute.For<IValidacaoDocumentosServico>();
            validadorDocumentos.EhCpfValido(Arg.Any<string>()).Returns(true);

            var usuarioServico = new UsuarioServico(null, null, null, validadorDocumentos, configuracaoServico);

            // Act and Assert
            usuarioServico.ValidarUsuario(new Usuario() { DataNascimento = DateTime.Now.AddDays(-idade * 365) });
        }
    }
}
