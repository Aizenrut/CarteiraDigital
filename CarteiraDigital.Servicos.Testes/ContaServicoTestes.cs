using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class ContaServicoTestes
    {
        [TestMethod]
        public void ValidarConta_IdInvalido_DeveLancarExcecao()
        {
            // Arrange
            int contaId = 1;

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaId).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            Action acao = () => contaServico.ValidarConta(contaId);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
        }

        [TestMethod]
        public void ValidarConta_IdValido_NaoDeveLancarExcecao()
        {
            // Arrange
            int contaId = 1;

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaId).Returns(true);

            var contaServico = new ContaServico(contaRepositorio);

            // Act and Assert
            contaServico.ValidarConta(contaId);
        }

        [TestMethod]
        public void ObterConta_IdValido_DeveRetornarAConta()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            var result = contaServico.ObterConta(conta.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(conta, result);
        }

        [TestMethod]
        public void VincularCashIn_PassandoPorParametro_DeveVincular()
        {
            // Arrange
            var conta = new Conta { CashIns = new List<CashIn>() };
            var cashIn = new CashIn();

            var contaRepositorio = Substitute.For<IContaRepositorio>();

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            contaServico.VincularCashIn(conta, cashIn);

            // Assert
            contaRepositorio.Received(1).Update(conta);
            Assert.IsTrue(conta.CashIns.Any());
            Assert.AreSame(cashIn, conta.CashIns.First());
        }

        [TestMethod]
        public void VincularCashOut_PassandoPorParametro_DeveVincular()
        {
            // Arrange
            var conta = new Conta { CashOuts = new List<CashOut>() };
            var cashOut = new CashOut();

            var contaRepositorio = Substitute.For<IContaRepositorio>();

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            contaServico.VincularCashOut(conta, cashOut);

            // Assert
            contaRepositorio.Received(1).Update(conta);
            Assert.IsTrue(conta.CashOuts.Any());
            Assert.AreSame(cashOut, conta.CashOuts.First());
        }

        [TestMethod]
        public void VincularTransferencia_PassandoPorParametro_DeveVincular()
        {
            // Arrange
            var conta = new Conta { Transferencias = new List<Transferencia>() };
            var transferencia = new Transferencia();

            var contaRepositorio = Substitute.For<IContaRepositorio>();

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            contaServico.VincularTransferencia(conta, transferencia);

            // Assert
            contaRepositorio.Received(1).Update(conta);
            Assert.IsTrue(conta.Transferencias.Any());
            Assert.AreSame(transferencia, conta.Transferencias.First());
        }

        [TestMethod]
        public void Nova_ContaVazia_DeveCriarESalvar()
        {
            // Arrange
            var usuario = "teste123";
            Conta contaCriada = null;

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.When(x => x.Post(Arg.Any<Conta>()))
                            .Do(x => contaCriada = x.Arg<Conta>());

            var contaServico = new ContaServico(contaRepositorio);

            // Act
            contaServico.Cadastrar(usuario);

            // Assert
            contaRepositorio.Received(1).Post(contaCriada);
            Assert.IsNotNull(contaCriada);
            Assert.AreEqual(usuario, contaCriada.UsuarioTitular);
        }
    }
}
