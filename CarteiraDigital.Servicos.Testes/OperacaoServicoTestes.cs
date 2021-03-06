﻿using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class OperacaoServicoTestes
    {
        [TestMethod]
        public void PodeAlterarStatus_StatusPendente_DeveRetornarTrue()
        {
            // Arrange
            var operacao = new CashIn();
            var operacaoServico = new OperacaoServico(null);

            // Act
            var resultado = operacaoServico.PodeAlterarStatus(operacao);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PodeAlterarStatus_StatusEfetivado_DeveRetornarFalse()
        {
            // Arrange
            var operacao = new CashIn { Status = StatusOperacao.Efetivada };
            var operacaoServico = new OperacaoServico(null);

            // Act
            var resultado = operacaoServico.PodeAlterarStatus(operacao);

            //Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PodeAlterarStatus_StatusComErro_DeveRetornarFalse()
        {
            // Arrange
            var operacao = new CashIn { Status = StatusOperacao.ComErro };
            var operacaoServico = new OperacaoServico(null);

            // Act
            var resultado = operacaoServico.PodeAlterarStatus(operacao);

            //Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void MarcarEfetivada_OperacaoPendente_DeveAlterarOStatus()
        {
            // Arrange
            var operacao = new CashIn();
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.MarcarEfetivada(operacao);

            //Assert
            Assert.AreEqual(StatusOperacao.Efetivada, operacao.Status);
        }

        [TestMethod]
        public void MarcarComErro_OperacaoPendente_DeveAlterarOStatus()
        {
            // Arrange
            var erro = "teste";
            var operacao = new CashIn();

            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.MarcarComErro(operacao, erro);

            //Assert
            Assert.AreEqual(StatusOperacao.ComErro, operacao.Status);
            Assert.AreEqual(erro, operacao.Erro);
        }

        [TestMethod]
        public void AlterarStatusTemplate_OperacaoPendenteStatusEfetivada_DeveAlterarOStatus()
        {
            // Arrange
            var operacao = new CashIn();
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.AlterarStatusTemplate(operacao, StatusOperacao.Efetivada);

            //Assert
            Assert.AreEqual(StatusOperacao.Efetivada, operacao.Status);
        }

        [TestMethod]
        public void AlterarStatusTemplate_OperacaoPendenteStatusComErro_DeveAlterarOStatus()
        {
            // Arrange
            var operacao = new CashIn();
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.AlterarStatusTemplate(operacao, StatusOperacao.ComErro);

            //Assert
            Assert.AreEqual(StatusOperacao.ComErro, operacao.Status);
        }

        [TestMethod]
        public void AlterarStatusTemplate_OperacaoEfetivadaStatusComErro_DeveLancarExcecao()
        {
            // Arrange
            var operacao = new CashIn { Status = StatusOperacao.Efetivada };
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.AlterarStatusTemplate(operacao, StatusOperacao.ComErro);

            //Assert
            Assert.AreEqual(StatusOperacao.ComErro, operacao.Status);
        }

        [TestMethod]
        public void AlterarStatusTemplate_OperacaoComErroStatusEfetivada_DeveLancarExcecao()
        {
            // Arrange
            var operacao = new CashIn { Status = StatusOperacao.ComErro };
            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.AlterarStatusTemplate(operacao, StatusOperacao.Efetivada);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("Não é possível alterar o status de uma operação Com erro!"));
            Assert.AreEqual(StatusOperacao.ComErro, operacao.Status);
        }

        [TestMethod]
        public void ValidarValor_ValorMenorQueZero_DeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.ValidarValor(-1);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
        }

        [TestMethod]
        public void ValidarValor_ValorIgualAZero_DeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.ValidarValor(0);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
        }

        [TestMethod]
        public void ValidarValor_ValorMaiorQueZero_NaoDeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act and Assert
            operacaoServico.ValidarValor(1);
        }

        [TestMethod]
        public void ValidarSaldo_SaldoInsuficiente_DeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.ValidarSaldo(new Conta(), 10);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O saldo da conta é insuficiente para realizar a operação!"));
        }

        [TestMethod]
        public void ValidarSaldo_SaldoMaiorQueOValor_NaoDeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.ValidarSaldo(new Conta { Saldo = 100 }, 10);
        }

        [TestMethod]
        public void ValidarSaldo_SaldoIgualAoValor_NaoDeveLancarExcecao()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.ValidarSaldo(new Conta { Saldo = 10 }, 10);
        }

        [TestMethod]
        public void Creditar_ValorValido_DeveCreditarOValor()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10;

            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.Creditar(conta, valor);

            // Assert
            Assert.AreEqual(valor, conta.Saldo);
        }

        [TestMethod]
        public void Creditar_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta();
            var valor = -1;

            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.Creditar(conta, valor);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            Assert.AreEqual(0, conta.Saldo);
        }

        [TestMethod]
        public void Debitar_SaldoSuficiente_DeveDebitarOValor()
        {
            // Arrange
            var conta = new Conta { Saldo = 100 };

            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.Debitar(conta, 10m);

            // Assert
            Assert.AreEqual(90, conta.Saldo);
        }

        [TestMethod]
        public void Debitar_SaldoInsuficiente_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Saldo = 10 };

            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.Debitar(conta, 100);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O saldo da conta é insuficiente para realizar a operação!"));
            Assert.AreEqual(10, conta.Saldo);
        }

        [TestMethod]
        public void Debitar_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Saldo = 100 };

            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.Debitar(conta, -1);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            Assert.AreEqual(100, conta.Saldo);
        }

        [TestMethod]
        public void ValidarArgumentoTemplate_CondicaoVerdadeira_DeveLancarArgumentException()
        {
            // Arrange
            var mensagem = "Teste unitário.";

            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.ValidarArgumentoTemplate(true, mensagem);

            //Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains(mensagem));
        }

        [TestMethod]
        public void ValidarArgumentoTemplate_CondicaoFalsa_NaoDeveLancarArgumentException()
        {
            // Arrange
            var operacaoServico = new OperacaoServico(null);

            // Act and Assert
            operacaoServico.ValidarArgumentoTemplate(false, "Teste unitário.");
        }

        [TestMethod]
        public void AlterarValoresTemplate_ValorInvalido_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Saldo = 1 };

            Action<Conta> acaoPrincipal = (conta) =>
            {
                conta.Saldo = 999;
            };

            var operacaoServico = new OperacaoServico(null);

            // Act
            Action acao = () => operacaoServico.AlterarValoresTemplate(conta, -1, acaoPrincipal);

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("O valor da operação deve ser superior a zero!"));
            Assert.AreEqual(1, conta.Saldo);
        }

        [TestMethod]
        public void AlterarValoresTemplate_Valido_DeveExecutarAcaoPrincipal()
        {
            // Arrange
            var conta = new Conta { Saldo = 1 };
            var valor = 999m;

            Action<Conta> acaoPrincipal = (conta) =>
            {
                conta.Saldo = valor;
            };

            var operacaoServico = new OperacaoServico(null);

            // Act
            operacaoServico.AlterarValoresTemplate(conta, valor, acaoPrincipal);

            // Assert
            Assert.AreEqual(valor, conta.Saldo);
        }

        [TestMethod]
        public void ValidarDescricao_DescricaoNula_DevePermitir()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);

            // Act and Assert
            operacaoServico.ValidarDescricao(null);
        }

        [TestMethod]
        public void ValidarDescricao_DescricaoVazia_DevePermitir()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);

            // Act and Assert
            operacaoServico.ValidarDescricao(string.Empty);
        }

        [TestMethod]
        public void ValidarDescricao_DescricaoMenorQueMaximo_DevePermitir()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);

            // Act and Assert
            operacaoServico.ValidarDescricao("a");
        }

        [TestMethod]
        public void ValidarDescricao_DescricaoIgualAoMaximo_DevePermitir()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);

            // Act and Assert
            operacaoServico.ValidarDescricao("abcde");
        }

        [TestMethod]
        public void ValidarDescricao_DescricaoMaiorQueMaximo_DeveLancarExcecao()
        {
            // Arrange
            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterTamanhoMaximoDescricao().Returns((short)5);

            var operacaoServico = new OperacaoServico(configuracaoServico);
            
            // Act
            Action acao = () => operacaoServico.ValidarDescricao("abcdef");

            // Assert
            var excecao = Assert.ThrowsException<CarteiraDigitalException>(acao);
            Assert.IsTrue(excecao.Message.Contains("A descrição não pode ter mais que 5 caracteres!"));
        }
    }
}
