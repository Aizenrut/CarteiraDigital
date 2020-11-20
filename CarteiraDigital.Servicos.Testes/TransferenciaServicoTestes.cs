using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class TransferenciaServicoTestes
    {
        [TestMethod]
        public void GerarPeloTipo_InformacoesValidasDeSaida_DeveGerarTransferencia()
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

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            transferenciaServico.GerarPeloTipo(conta.Id, valor, "Teste unitário.", tipoTransferencia);

            // Assert
            Assert.IsNotNull(transferenciaGerada);
            Assert.AreEqual(tipoTransferencia, transferenciaGerada.TipoMovimentacao);
            Assert.AreEqual(StatusOperacao.Pendente, transferenciaGerada.Status);
            Assert.AreEqual(valor, transferenciaGerada.SaldoAnterior);
            Assert.AreEqual(valor, transferenciaGerada.Valor);
            Assert.AreNotEqual(default, transferenciaGerada.Data);
        }

        [TestMethod]
        public void GerarPeloTipo_InformacoesValidasDeEntrada_DeveGerarTransferencia()
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

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            transferenciaServico.GerarPeloTipo(conta.Id, valor, "Teste unitário.", tipoTransferencia);

            // Assert
            Assert.IsNotNull(transferenciaGerada);
            Assert.AreEqual(tipoTransferencia, transferenciaGerada.TipoMovimentacao);
            Assert.AreEqual(StatusOperacao.Pendente, transferenciaGerada.Status);
            Assert.AreEqual(0, transferenciaGerada.SaldoAnterior);
            Assert.AreEqual(valor, transferenciaGerada.Valor);
            Assert.AreNotEqual(default, transferenciaGerada.Data);
        }

        [TestMethod]
        public void Efetivar_InformacoesValidas_EfetivarAsDuasTransferencias()
        {
            // Arrange
            var valor = 10m;
            
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = valor
            };
            
            var contaDestino = new Conta { Id = 2 };

            var transferenciaSaida = new Transferencia
            {
                Id = 1,
                Valor = 10m,
                ContaId = contaOrigem.Id
            };

            var transferenciaEntrada = new Transferencia
            {
                Id = 2,
                Valor = 10m,
                TipoMovimentacao = TipoMovimentacao.Entrada,
                ContaId = contaDestino.Id
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferenciaEntrada.Id).Returns(transferenciaEntrada);
            transferenciaRepositorio.Get(transferenciaSaida.Id).Returns(transferenciaSaida);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);
            contaRepositorio.Get(contaDestino.Id).Returns(contaDestino);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico, null);

            // Act
            transferenciaServico.Efetivar(new EfetivarOperacaoBinariaDto(transferenciaSaida.Id, transferenciaEntrada.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(1).Finalizar();
            Assert.AreEqual(StatusOperacao.Efetivada, transferenciaSaida.Status);
            Assert.AreEqual(StatusOperacao.Efetivada, transferenciaEntrada.Status);
            Assert.AreEqual(0, contaOrigem.Saldo);
            Assert.AreEqual(10, contaDestino.Saldo);
        }

        [TestMethod]
        public void Efetivar_ContaOrigemInvalida_DeveGravarErro()
        {
            // Arrange
            var transferenciaSaida = new Transferencia
            {
                Id = 1,
                Valor = 10
            };

            var transferenciaEntrada = new Transferencia
            {
                Id = 2,
                Valor = 10
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferenciaEntrada.Id).Returns(transferenciaEntrada);
            transferenciaRepositorio.Get(transferenciaSaida.Id).Returns(transferenciaSaida);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico, null);

            // Act
            transferenciaServico.Efetivar(new EfetivarOperacaoBinariaDto(transferenciaSaida.Id, transferenciaEntrada.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaSaida.Status);
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaEntrada.Status);
            Assert.AreEqual("A conta informada é inválida!", transferenciaSaida.Erro);
            Assert.AreEqual("A conta informada é inválida!", transferenciaSaida.Erro);
        }

        [TestMethod]
        public void Efetivar_ContaDestinoInvalida_DeveGravarErro()
        {
            // Arrange
            var contaOrigem = new Conta
            {
                Id = 1,
                Saldo = 10m
            };

            var contaDestino = new Conta { Id = 2 };

            var transferenciaSaida = new Transferencia
            {
                Id = 1,
                Valor = 10m,
                ContaId = contaOrigem.Id
            };

            var transferenciaEntrada = new Transferencia
            {
                Id = 2,
                Valor = 10m,
                ContaId = contaDestino.Id
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferenciaEntrada.Id).Returns(transferenciaEntrada);
            transferenciaRepositorio.Get(transferenciaSaida.Id).Returns(transferenciaSaida);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);
            contaRepositorio.Any(contaDestino.Id).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico, null);

            // Act
            transferenciaServico.Efetivar(new EfetivarOperacaoBinariaDto(transferenciaSaida.Id, transferenciaEntrada.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaSaida.Status);
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaEntrada.Status);
            Assert.AreEqual("A conta informada é inválida!", transferenciaSaida.Erro);
            Assert.AreEqual("A conta informada é inválida!", transferenciaSaida.Erro);
        }

        [TestMethod]
        public void Efetivar_ValorInvalido_DeveGravarErro()
        {
            // Arrange
            var contaOrigem = new Conta { Id = 1 };

            var transferenciaSaida = new Transferencia
            {
                Id = 1,
                Valor = 0,
                ContaId = contaOrigem.Id
            };

            var transferenciaEntrada = new Transferencia
            {
                Id = 2,
                Valor = 0
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferenciaEntrada.Id).Returns(transferenciaEntrada);
            transferenciaRepositorio.Get(transferenciaSaida.Id).Returns(transferenciaSaida);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico, null);

            // Act
            transferenciaServico.Efetivar(new EfetivarOperacaoBinariaDto(transferenciaSaida.Id, transferenciaEntrada.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaSaida.Status);
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaEntrada.Status);
            Assert.AreEqual("O valor da operação deve ser superior a zero!", transferenciaSaida.Erro);
            Assert.AreEqual("O valor da operação deve ser superior a zero!", transferenciaSaida.Erro);
        }

        [TestMethod]
        public void Efetivar_SaldoInsuficiente_DeveGravarErro()
        {
            // Arrange
            var contaOrigem = new Conta
            {
                Id = 1,
            };

            var transferenciaSaida = new Transferencia
            {
                Id = 1,
                Valor = 10,
                ContaId = contaOrigem.Id
            };

            var transferenciaEntrada = new Transferencia
            {
                Id = 2,
                Valor = 10
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferenciaEntrada.Id).Returns(transferenciaEntrada);
            transferenciaRepositorio.Get(transferenciaSaida.Id).Returns(transferenciaSaida);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(contaOrigem.Id).Returns(true);
            contaRepositorio.Get(contaOrigem.Id).Returns(contaOrigem);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, transacaoServico, null);

            // Act
            transferenciaServico.Efetivar(new EfetivarOperacaoBinariaDto(transferenciaSaida.Id, transferenciaEntrada.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(0).Finalizar();
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaSaida.Status);
            Assert.AreEqual(StatusOperacao.ComErro, transferenciaEntrada.Status);
            Assert.AreEqual("O saldo da conta é insuficiente para realizar a operação!", transferenciaSaida.Erro);
            Assert.AreEqual("O saldo da conta é insuficiente para realizar a operação!", transferenciaSaida.Erro);
        }

        [TestMethod]
        public void Efetivar_TransferenciaValidaPassandoTransferencia_DeveMarcarComoEfetivada()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Saldo = 10m
            };

            var transferencia = new Transferencia
            {
                Id = 1,
                Valor = 10m,
                ContaId = conta.Id
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferencia.Id).Returns(transferencia);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            transferenciaServico.Efetivar(transferencia);

            // Assert
            Assert.AreEqual(StatusOperacao.Efetivada, transferencia.Status);
            Assert.AreEqual(0, conta.Saldo);
        }

        [TestMethod]
        public void Efetivar_ContaInvalidaPassandoTransferencia_DeveLancarExcecao()
        {
            // Arrange
            var transferencia = new Transferencia
            {
                Id = 1,
                Valor = 10
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferencia.Id).Returns(transferencia);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(Arg.Any<int>()).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            Action acao = () => transferenciaServico.Efetivar(transferencia);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A conta informada é inválida!"));
        }

        [TestMethod]
        public void Efetivar_ValorInvalidoPassandoTransferencia_DeveLancarExcecao()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            var transferencia = new Transferencia
            {
                Id = 2,
                Valor = 0,
                ContaId = conta.Id
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferencia.Id).Returns(transferencia);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            Action acao = () => transferenciaServico.Efetivar(transferencia);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
        }

        [TestMethod]
        public void Efetivar_SaldoInsuficientePassandoTransferencia_DeveLancarExcecao()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
            };

            var transferencia = new Transferencia
            {
                Id = 1,
                Valor = 10,
                ContaId = conta.Id
            };

            var transferenciaRepositorio = Substitute.For<ITransferenciaRepositorio>();
            transferenciaRepositorio.Get(transferencia.Id).Returns(transferencia);

            var operacaoServico = new OperacaoServico(null);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);

            var transferenciaServico = new TransferenciaServico(transferenciaRepositorio, operacaoServico, contaServico, null, null);

            // Act
            Action acao = () => transferenciaServico.Efetivar(transferencia);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O saldo da conta é insuficiente para realizar a operação!"));
        }

        [TestMethod]
        public async Task Gerar_DescricaoMaiorQueMaximo_DeveLancarExcecao()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);

            var transferenciaServico = new TransferenciaServico(null, operacaoServico, null, null, null);

            // Act
            Func<Task> acao = async () => await transferenciaServico.Gerar(new OperacaoBinariaDto(1, 2, 10, "abcdef"));

            // Assert
            var excecao = await Assert.ThrowsExceptionAsync<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A descrição não pode ter mais que 5 caracteres!"));
        }
    }
}
