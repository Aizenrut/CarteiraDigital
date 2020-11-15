using CarteiraDigital.ProvedorAutenticacao.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarteiraDigital.ProvedorAutenticacao.Testes.Builders
{
    [TestClass]
    public class UsuarioBuilderTestes
    {
        [TestMethod]
        public void ComNomeUsuario_InformandoNomeUsuario_DeveGerarComNomeUsuario()
        {
            // Arrange
            var nomeUsuario = "teste123";

            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.ComNomeUsuario(nomeUsuario).Gerar();

            // Assert
            Assert.AreEqual(nomeUsuario, usuario.UserName);
        }

        [TestMethod]
        public void ComNome_InformandoNome_DeveGerarComNome()
        {
            // Arrange
            var nome = "Teste";

            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.ComNome(nome).Gerar();

            // Assert
            Assert.AreEqual(nome, usuario.Nome);
        }

        [TestMethod]
        public void ComSobrenome_InformandoSobrenome_DeveGerarComSobrenome()
        {
            // Arrange
            var sobrenome = "Unitário";

            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.ComSobrenome(sobrenome).Gerar();

            // Assert
            Assert.AreEqual(sobrenome, usuario.Sobrenome);
        }

        [TestMethod]
        public void ComCpf_InformandoCpf_DeveGerarComCpfNaoFormatado()
        {
            // Arrange
            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.ComCpf("000.000.000-00").Gerar();

            // Assert
            Assert.AreEqual("00000000000", usuario.Cpf);
        }

        [TestMethod]
        public void NascidoEm_InformandoDataNascimento_DeveGerarComDataNascimento()
        {
            // Arrange
            var dataNascimento = DateTime.Now;

            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.NascidoEm(dataNascimento).Gerar();

            // Assert
            Assert.AreEqual(dataNascimento, usuario.DataNascimento);
        }

        [TestMethod]
        public void Gerar_InformandoTodosOsCampos_DeveGerarComTodosOsCampos()
        {
            // Arrange
            var nomeUsuario = "teste123";
            var nome = "Teste";
            var sobrenome = "Unitário";
            var dataNascimento = DateTime.Now;

            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.ComNomeUsuario(nomeUsuario)
                                 .ComNome(nome)
                                 .ComSobrenome(sobrenome)
                                 .ComCpf("000.000.000-00")
                                 .NascidoEm(dataNascimento)
                                 .Gerar();

            // Assert
            Assert.AreEqual(nomeUsuario, usuario.UserName);
            Assert.AreEqual(nome, usuario.Nome);
            Assert.AreEqual(sobrenome, usuario.Sobrenome);
            Assert.AreEqual("00000000000", usuario.Cpf);
            Assert.AreEqual(dataNascimento, usuario.DataNascimento);
        }

        [TestMethod]
        public void Gerar_InformandoNenhumCampo_DeveGerarSemCamposPreenchidos()
        {
            // Arrange
            var builder = new UsuarioBuilder();

            // Act
            var usuario = builder.Gerar();

            // Assert
            Assert.AreEqual(default, usuario.UserName);
            Assert.AreEqual(default, usuario.Nome);
            Assert.AreEqual(default, usuario.Sobrenome);
            Assert.AreEqual(default, usuario.Cpf);
            Assert.AreEqual(default, usuario.DataNascimento);
        }
    }
}
