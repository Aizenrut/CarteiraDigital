using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class CashInServicoTestes
    {
        [TestMethod]
        public void EhPrimeiroCashIn_Nao_DeveRetornarFalse()
        {
            // Arrange
            int contaId = 1;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(true);

            var cashInServico = new CashInServico(cashInRepositorio, null, null);

            // Act
            var resultado = cashInServico.EhPrimeiroCashIn(contaId);

            //Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EhPrimeiroCashIn_Sim_DeveRetornarTrue()
        {
            // Arrange
            int contaId = 1;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(false);

            var cashInServico = new CashInServico(cashInRepositorio, null, null);

            // Act
            var resultado = cashInServico.EhPrimeiroCashIn(contaId);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void GerarCashIn_PrimeiroCashInValido_DeveRetornarOCashInComBonificacao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var descricao = "Teste unitário.";

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(false);

            var cashInServico = new CashInServico(cashInRepositorio, null, null);

            // Act
            var result = cashInServico.GerarCashIn(conta, 10m, descricao);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(11m, result.Valor);
            Assert.AreEqual(descricao, result.Descricao);
            Assert.AreNotEqual(default, result.Data);
        }

        [TestMethod]
        public void GerarCashIn_CashInValido_DeveRetornarOCashInSemBonificacao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10m;
            var descricao = "Teste unitário.";

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(true);

            var cashInServico = new CashInServico(cashInRepositorio, null, null);

            // Act
            var result = cashInServico.GerarCashIn(conta, valor, descricao);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(valor, result.Valor);
            Assert.AreEqual(descricao, result.Descricao);
            Assert.AreNotEqual(default, result.Data);
        }

        [TestMethod]
        public void Efetivar_PrimeiroCashInValido_DeveRetornarOCashInComBonificacao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var descricao = "Teste unitário.";
            CashIn cashInGerado = null;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(false);
            cashInRepositorio.When(x => x.Post(Arg.Any<CashIn>()))
                             .Do(x => cashInGerado = x.Arg<CashIn>());

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var dto = new OperacaoUnariaDto(conta.Id, 10m, descricao);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico);

            // Act
            cashInServico.Efetivar(dto);

            // Assert
            Assert.IsNotNull(cashInGerado);
            Assert.AreEqual(11m, cashInGerado.Valor);
            Assert.AreEqual(descricao, cashInGerado.Descricao);
            Assert.AreNotEqual(default, cashInGerado.Data);
        }

        [TestMethod]
        public void Efetivar_CashInValido_DeveRetornarOCashInSemBonificacao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10m;
            var descricao = "Teste unitário.";
            CashIn cashInGerado = null;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(true);
            cashInRepositorio.When(x => x.Post(Arg.Any<CashIn>()))
                             .Do(x => cashInGerado = x.Arg<CashIn>());

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var dto = new OperacaoUnariaDto(conta.Id, valor, descricao);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico);

            // Act
            cashInServico.Efetivar(dto);

            // Assert
            Assert.IsNotNull(cashInGerado);
            Assert.AreEqual(valor, cashInGerado.Valor);
            Assert.AreEqual(descricao, cashInGerado.Descricao);
            Assert.AreNotEqual(default, cashInGerado.Data);
        }

        [TestMethod]
        public void Efetivar_ContaInvalida_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10m;
            var descricao = "Teste unitário.";
            CashIn cashInGerado = null;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.When(x => x.Post(Arg.Any<CashIn>()))
                             .Do(x => cashInGerado = x.Arg<CashIn>());

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var dto = new OperacaoUnariaDto(conta.Id, valor, descricao);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico);

            // Act
            Action acao = () => cashInServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
            Assert.IsNull(cashInGerado);
        }

        [TestMethod]
        public void Efetivar_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 0;
            var descricao = "Teste unitário.";
            CashIn cashInGerado = null;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.When(x => x.Post(Arg.Any<CashIn>()))
                             .Do(x => cashInGerado = x.Arg<CashIn>());

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var dto = new OperacaoUnariaDto(conta.Id, valor, descricao);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico);

            // Act
            Action acao = () => cashInServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            Assert.IsNull(cashInGerado);
            cashInRepositorio.Received(0).Post(Arg.Any<CashIn>());
        }
    }
}
