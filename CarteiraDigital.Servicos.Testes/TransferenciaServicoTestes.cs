using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class TransferenciaServicoTestes
    {
        [TestMethod]
        public void TransferirPeloTipo_ContaInvalida_DeveLancarExcecaoEPararProcesso()
        {
            // Arrange
            var valor = 10m;

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            var operacaoServico = Substitute.For<IOperacaoServico>();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null);

            // Act
            Action acao = () => transferenciaServico.TransferirPeloTipo(1, valor, "Teste unitário.", TipoMovimentacao.Saida);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
            operacaoServico.Received(0).Debitar(Arg.Any<Conta>(), valor);
            transferenciaRepositorio.Received(0).Post(Arg.Any<Transferencia>());
            contaRepositorio.Received(0).Update(Arg.Any<Conta>());
        }

        [TestMethod]
        public void TransferirPeloTipo_ValorInvalido_DeveLancarExcecaoEPararProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 0m;

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null);

            // Act
            Action acao = () => transferenciaServico.TransferirPeloTipo(1, valor, "Teste unitário.", TipoMovimentacao.Saida);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            transferenciaRepositorio.Received(0).Post(Arg.Any<Transferencia>());
            contaRepositorio.Received(0).Update(Arg.Any<Conta>());
        }

        [TestMethod]
        public void TransferirPeloTipo_SaldoInsuficiente_DeveLancarExcecaoEPararProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10m;

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null);

            // Act
            Action acao = () => transferenciaServico.TransferirPeloTipo(1, valor, "Teste unitário.", TipoMovimentacao.Saida);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O saldo da conta é insuficiente para realizar a operação!"));
            transferenciaRepositorio.Received(0).Post(Arg.Any<Transferencia>());
            contaRepositorio.Received(0).Update(Arg.Any<Conta>());
        }

        [TestMethod]
        public void TransferirPeloTipo_InformacoesValidasDeSaida_DeveRealizarOperacaoDeSaida()
        {
            // Arrange
            var valor = 10m;
            var conta = new Conta 
            {
                Id = 1,
                Saldo = valor
            };

            var tipoTransferencia = TipoMovimentacao.Saida;
            Transferencia transferenciaGerada = null;

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciaGerada = x.Arg<Transferencia>());

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null);

            // Act
            transferenciaServico.TransferirPeloTipo(conta.Id, valor, "Teste unitário.", tipoTransferencia);

            // Assert
            Assert.IsNotNull(transferenciaGerada);
            Assert.AreEqual(tipoTransferencia, transferenciaGerada.TipoMovimentacao);
            Assert.AreEqual(valor, transferenciaGerada.SaldoAnterior);
            Assert.AreEqual(valor, transferenciaGerada.Valor);
            Assert.AreNotEqual(default, transferenciaGerada.Data);
            Assert.AreEqual(0, conta.Saldo);
        }

        [TestMethod]
        public void TransferirPeloTipo_InformacoesValidasDeEntrada_DeveRealizarOperacaoDeEntrada()
        {
            // Arrange
            var valor = 10m;
            var conta = new Conta
            {
                Id = 1,
            };

            var tipoTransferencia = TipoMovimentacao.Entrada;
            Transferencia transferenciaGerada = null;

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciaGerada = x.Arg<Transferencia>());

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null);

            // Act
            transferenciaServico.TransferirPeloTipo(conta.Id, valor, "Teste unitário.", tipoTransferencia);

            // Assert
            Assert.IsNotNull(transferenciaGerada);
            Assert.AreEqual(tipoTransferencia, transferenciaGerada.TipoMovimentacao);
            Assert.AreEqual(0, transferenciaGerada.SaldoAnterior);
            Assert.AreEqual(valor, transferenciaGerada.Valor);
            Assert.AreNotEqual(default, transferenciaGerada.Data);
            Assert.AreEqual(valor, conta.Saldo);
        }

        [TestMethod]
        public void Efetivar_InformacoesValidas_DeveCriarDuasTranferencias()
        {
            // Arrange
            var valor = 10m;
            var descricao = "Teste unitário.";
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = valor
            };

            var contaDestino = new Conta { Id = 2 };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);
            contaRepositorio.Get(contaDestino.Id).Returns(contaDestino);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, contaDestino.Id, valor, descricao);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            transferenciaServico.Efetivar(dto);

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(1).Finalizar();
            Assert.IsNotNull(transferenciasGeradas);
            Assert.AreEqual(2, transferenciasGeradas.Count);
            Assert.AreEqual(TipoMovimentacao.Saida, transferenciasGeradas[0].TipoMovimentacao);
            Assert.AreEqual(TipoMovimentacao.Entrada, transferenciasGeradas[1].TipoMovimentacao);
        }

        [TestMethod]
        public void Efetivar_ContaOrigemInvalida_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var valor = 10m;
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = valor
            };

            var contaDestino = new Conta { Id = 2 };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(false);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, contaDestino.Id, valor, "Teste unitário.");

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            Action acao = () => transferenciaServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }

        [TestMethod]
        public void Efetivar_ContaDestinoInvalida_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var valor = 10m;
            var descricao = "Teste unitário.";
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = valor
            };

            var contaDestino = new Conta { Id = 2 };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);
            contaRepositorio.Any(contaDestino.Id).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, contaDestino.Id, valor, descricao);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            Action acao = () => transferenciaServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }

        [TestMethod]
        public void Efetivar_InformacoesValidasDeEntrada_DeveRealizarAsOperacoesDeSaidaEEntrada()
        {
            // Arrange
            var valor = 10m;
            var descricao = "Teste unitário.";
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = valor
            };

            var contaDestino = new Conta { Id = 2 };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(false);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, contaDestino.Id, valor, descricao);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            Action acao = () => transferenciaServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }

        [TestMethod]
        public void Efetivar_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var contaOrigem = new Conta { Id = 1 };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, 2, 0, "Teste unitário.");

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            Action acao = () => transferenciaServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }

        [TestMethod]
        public void Efetivar_SaldoInsuficiente_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var valor = 10m;
            var contaOrigem = new Conta
            {
                Id = 1,
            };

            var transferenciasGeradas = new List<Transferencia>();

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.When(x => x.Post(Arg.Any<Transferencia>()))
                                    .Do(x => transferenciasGeradas.Add(x.Arg<Transferencia>()));

            var operacaoServico = new OperacaoServico();

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var dto = new OperacaoBinariaDto(contaOrigem.Id, 2, valor, "Teste unitário.");

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico);

            // Act
            Action acao = () => transferenciaServico.Efetivar(dto);

            // Assert
            var excecao = Assert.ThrowsException<ArgumentException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O saldo da conta é insuficiente para realizar a operação!"));
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }
    }
}
