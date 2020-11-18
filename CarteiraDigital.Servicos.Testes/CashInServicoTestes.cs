using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos.Clients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

            var cashInServico = new CashInServico(cashInRepositorio, null, null, null, null, null);

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

            var cashInServico = new CashInServico(cashInRepositorio, null, null, null, null, null);

            // Act
            var resultado = cashInServico.EhPrimeiroCashIn(contaId);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public async Task Gerar_PrimeiroCashInValido_DeveRetornarOCashInComBonificacao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var descricao = "Teste unitário.";

            CashIn cashInGerado = null;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(false);
            cashInRepositorio.When(x => x.Post(Arg.Any<CashIn>()))
                             .Do(x => cashInGerado = x.Arg<CashIn>());

            var contaServico = Substitute.For<IContaServico>();
            contaServico.ObterConta(conta.Id).Returns(conta);

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var produtorClient = Substitute.For<IProdutorOperacoesClient>();

            var cashInServico = new CashInServico(cashInRepositorio, null, contaServico, configuracaoServico, null, produtorClient);

            // Act
            await cashInServico.Gerar(new OperacaoUnariaDto(conta.Id, 10m, descricao));

            // Assert
            Assert.IsNotNull(cashInGerado);
            Assert.AreEqual(10m, cashInGerado.Valor);
            Assert.AreEqual(1m, cashInGerado.ValorBonificacao);
            Assert.AreEqual(descricao, cashInGerado.Descricao);
            Assert.AreEqual(conta.Id, cashInGerado.ContaId);
            Assert.AreNotEqual(default, cashInGerado.Data);
        }

        [TestMethod]
        public async Task GerarCashIn_CashInValido_DeveRetornarOCashInSemBonificacao()
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

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var produtorClient = Substitute.For<IProdutorOperacoesClient>();

            var contaServico = new ContaServico(contaRepositorio);

            var cashInServico = new CashInServico(cashInRepositorio, null, contaServico, configuracaoServico, null, produtorClient);

            // Act
            await cashInServico.Gerar(new OperacaoUnariaDto(conta.Id, valor, descricao));

            // Assert
            Assert.IsNotNull(cashInGerado);
            Assert.AreEqual(valor, cashInGerado.Valor);
            Assert.AreEqual(descricao, cashInGerado.Descricao);
            Assert.AreEqual(conta.Id, cashInGerado.ContaId);
            Assert.AreNotEqual(default, cashInGerado.Data);
        }

        [TestMethod]
        public void Efetivar_CashInValido_DeveEfetivar()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var cashIn = new CashIn
            {
                Id = 1,
                Valor = 10,
                ContaId = conta.Id
            };

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Get(cashIn.Id).Returns(cashIn);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = Substitute.For<IContaServico>();
            contaServico.ObterConta(conta.Id).Returns(conta);

            var operacaoServico = new OperacaoServico();

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico, configuracaoServico, transacaoServico, null);

            // Act
            cashInServico.Efetivar(new EfetivarOperacaoUnariaDto(cashIn.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(1).Finalizar();
            Assert.AreEqual(StatusOperacao.Efetivada, cashIn.Status);
            Assert.AreEqual(cashIn.Valor, conta.Saldo);
            cashInRepositorio.Received(1).Update(cashIn);
        }

        [TestMethod]
        public void Efetivar_ContaInvalida_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var cashIn = new CashIn
            {
                Id = 1,
                Valor = 10
            };

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Get(cashIn.Id).Returns(cashIn);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico, null, transacaoServico, null);

            // Act
            cashInServico.Efetivar(new EfetivarOperacaoUnariaDto(cashIn.Id));

            // Assert
            Assert.AreEqual(StatusOperacao.ComErro, cashIn.Status);
            Assert.AreEqual("A conta informada é inválida!", cashIn.Erro);
            transacaoServico.Received(0).GerarNova();
            transacaoServico.Received(0).Finalizar();
            cashInRepositorio.Received(1).Update(cashIn);
        }

        [TestMethod]
        public void Efetivar_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var cashIn = new CashIn
            {
                Id = 1,
                Valor = 0,
                ContaId = conta.Id
            };

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Get(cashIn.Id).Returns(cashIn);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var cashInServico = new CashInServico(cashInRepositorio, operacaoServico, contaServico, configuracaoServico, transacaoServico, null);

            // Act
            cashInServico.Efetivar(new EfetivarOperacaoUnariaDto(cashIn.Id));

            // Assert
            Assert.AreEqual("O valor da operação deve ser superior a zero!", cashIn.Erro);
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
            cashInRepositorio.Received(1).Update(cashIn);
        }

        [TestMethod]
        public void ObterBonificacao_PrimeiroCashIn_DeveRetornarABonificacao()
        {
            // Arrange
            var contaId = 1;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(false);

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var cashInServico = new CashInServico(cashInRepositorio, null, null, configuracaoServico, null, null);

            // Act
            var resultado = cashInServico.ObterBonificacao(contaId, 10);

            // Assert
            Assert.AreEqual(1m, resultado);
        }

        [TestMethod]
        public void ObterBonificacao_SegundoCashIn_DeveRetornarZero()
        {
            // Arrange
            var contaId = 1;

            var cashInRepositorio = Substitute.For<ICashInRepositorio>();
            cashInRepositorio.Any(Arg.Any<Expression<Func<CashIn, bool>>>()).Returns(true);

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualBonificacao().Returns(0.1m);

            var cashInServico = new CashInServico(cashInRepositorio, null, null, configuracaoServico, null, null);

            // Act
            var resultado = cashInServico.ObterBonificacao(contaId, 10);

            // Assert
            Assert.AreEqual(0, resultado);
        }
    }
}
