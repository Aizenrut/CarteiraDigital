using CarteiraDigital.Dados.Expressoes;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarteiraDigital.Dados.Testes.Expressoes
{
    [TestClass]
    public class OperacaoExpressaoTeste
    {
        private ICollection<Operacao> ObterListaOperacoes()
        {
            return new List<Operacao>
            {
                new CashIn() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new CashIn() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new CashIn() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new CashIn() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new CashIn() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new CashIn() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new CashIn() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new CashIn() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new CashIn() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new CashIn() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },
                new CashIn() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new CashIn() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },

                new CashOut() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new CashOut() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new CashOut() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new CashOut() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new CashOut() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new CashOut() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new CashOut() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new CashOut() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new CashOut() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new CashOut() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },
                new CashOut() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new CashOut() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },

                new Transferencia() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new Transferencia() { ContaId = 1, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(1) },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.Pendente, Data = DateTime.Now.AddDays(-1) },
                new Transferencia() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new Transferencia() { ContaId = 1, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.Efetivada, Data = DateTime.Now.AddDays(-1) },
                new Transferencia() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new Transferencia() { ContaId = 1, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now },
                new Transferencia() { ContaId = 2, Status = StatusOperacao.ComErro, Data = DateTime.Now.AddDays(-1) },
            };
        }

        private ICollection<OperacaoDto> ObterListaOperacoesDto()
        {
            return new List<OperacaoDto>
            {
                new OperacaoDto() {Status = StatusOperacao.Pendente },
                new OperacaoDto() {Status = StatusOperacao.Efetivada },
                new OperacaoDto() {Status = StatusOperacao.ComErro }
            };
        }

        [TestMethod]
        public void DaConta_FiltrandoPelaConta1_DeveRetornar18OperacoesDaConta1()
        {
            // Arrange
            var contaId = 1;
            var operacaoExpressao = new OperacaoExpressao();

            // Act
            var resultado = ObterListaOperacoes().Where(operacaoExpressao.DaConta<Operacao>(contaId).Compile());

            // Assert
            Assert.AreEqual(18, resultado.Count());
            Assert.IsTrue(resultado.All(x => x.ContaId == contaId));
        }

        [TestMethod]
        public void Efetivada_DeveRetornar1OperacaoEfetivada()
        {
            // Arrange
            var operacaoExpressao = new OperacaoExpressao();

            // Act
            var resultado = ObterListaOperacoesDto().Where(operacaoExpressao.Efetivada());

            // Assert
            Assert.AreEqual(1, resultado.Count());
            Assert.IsTrue(resultado.All(x => x.Status == StatusOperacao.Efetivada));
        }

        [TestMethod]
        public void NoPeriodo_FiltrandoPelaSemanaPassada_DeveRetornarListaVazia()
        {
            // Arrange
            var operacaoExpressao = new OperacaoExpressao();

            // Act
            var resultado = ObterListaOperacoes().Where(operacaoExpressao.NoPeriodo<Operacao>(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-6)).Compile());

            // Assert
            Assert.AreEqual(0, resultado.Count());
        }

        [TestMethod]
        public void NoPeriodo_FiltrandoPelosUltimos2Dias_DeveRetornar18Operacoes()
        {
            // Arrange
            var dataInicial = DateTime.Now.AddDays(-2);
            var dataFinal = DateTime.Now;

            var operacaoExpressao = new OperacaoExpressao();

            // Act
            var resultado = ObterListaOperacoes().Where(operacaoExpressao.NoPeriodo<Operacao>(dataInicial, dataFinal).Compile());

            // Assert
            Assert.AreEqual(18, resultado.Count());
            Assert.IsTrue(resultado.All(x => x.Data >= dataInicial && x.Data <= dataFinal));
        }

        [TestMethod]
        public void DaContaNoPeriodo_FiltrandoPelosUltimos2DiasDaConta1_DeveRetornar3Operacoes()
        {
            // Arrange
            var contaId = 1;
            var dataInicial = DateTime.Now.AddDays(-2);
            var dataFinal = DateTime.Now;

            var operacaoExpressao = new OperacaoExpressao();

            // Act
            var resultado = ObterListaOperacoes().Where(operacaoExpressao.DaContaNoPeriodo<Operacao>(contaId, dataInicial, dataFinal).Compile());

            // Assert
            Assert.AreEqual(9, resultado.Count());
            Assert.IsTrue(resultado.All(x => x.ContaId == contaId));
            Assert.IsTrue(resultado.All(x => x.Data >= dataInicial && x.Data <= dataFinal));
        }
    }
}
