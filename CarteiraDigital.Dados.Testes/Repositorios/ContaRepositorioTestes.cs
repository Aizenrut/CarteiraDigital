using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using CarteiraDigital.Models.Enumeracoes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarteiraDigital.Dados.Testes.Repositorios
{
    [TestClass]
    public class ContaRepositorioTestes
    {
        private ICollection<OperacaoDto> ObterOperacoesFinalizandoComCashOut()
        {
            var cashIn = new OperacaoDto
            {
                Data = DateTime.Now.AddDays(-2),
                Valor = 100,
                Descricao = "Teste CashIn",
                TipoMovimentacao = TipoMovimentacao.Entrada,
                TipoOperacao = TipoOperacao.CashIn,
                SaldoAnterior = 0
            };

            var transferencia = new OperacaoDto
            {
                Data = DateTime.Now.AddDays(-1),
                Valor = 50,
                Descricao = "Teste Transferência",
                TipoMovimentacao = TipoMovimentacao.Saida,
                TipoOperacao = TipoOperacao.Transferencia,
                SaldoAnterior = 100
            };

            var cashOut = new OperacaoDto
            {
                Data = DateTime.Now,
                Valor = 10,
                Descricao = "Teste CashOut",
                TipoMovimentacao = TipoMovimentacao.Saida,
                TipoOperacao = TipoOperacao.CashOut,
                SaldoAnterior = 50
            };

            return new List<OperacaoDto>
            {
                cashIn,
                transferencia,
                cashOut
            }.OrderByDescending(x => x.Data).ToList();
        }

        private ICollection<OperacaoDto> ObterOperacoesApenasEntrada()
        {
            var cashIn = new OperacaoDto
            {
                Data = DateTime.Now.AddDays(-2),
                Valor = 100,
                Descricao = "Teste CashIn",
                TipoMovimentacao = TipoMovimentacao.Entrada,
                TipoOperacao = TipoOperacao.CashIn,
                SaldoAnterior = 100
            };

            var transferencia = new OperacaoDto
            {
                Data = DateTime.Now.AddDays(-1),
                Valor = 50,
                Descricao = "Teste Transferência",
                TipoMovimentacao = TipoMovimentacao.Entrada,
                TipoOperacao = TipoOperacao.Transferencia,
                SaldoAnterior = 200
            };

            return new List<OperacaoDto>
            {
                cashIn,
                transferencia
            }.OrderByDescending(x => x.Data).ToList();
        }

        [TestMethod]
        public void ObterSaldoFinal_OperacoesFinalizandoComCashOut_DeveRetornar40()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoFinal(ObterOperacoesFinalizandoComCashOut());

            // Assert
            Assert.AreEqual(40, resultado);
        }

        [TestMethod]
        public void ObterSaldoFinal_OperacoesApenasEntrada_DeveRetornar250()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoFinal(ObterOperacoesApenasEntrada());

            // Assert
            Assert.AreEqual(250, resultado);
        }

        [TestMethod]
        public void ObterSaldoFinal_NenhumaOperacao_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoFinal(new List<OperacaoDto>());

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoFinal_PassandoNulo_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoFinal(null);

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoInicial_OperacoesFinalizandoComCashOut_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoInicial(ObterOperacoesFinalizandoComCashOut());

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoInicial_OperacoesApenasEntrada_DeveRetornar100()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoInicial(ObterOperacoesApenasEntrada());

            // Assert
            Assert.AreEqual(100, resultado);
        }

        [TestMethod]
        public void ObterSaldoInicial_NenhumaOperacao_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoInicial(new List<OperacaoDto>());

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoInicial_PassandoNulo_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoInicial(null);

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoTemplate_PassandoNulo_DeveExecutarAcao()
        {
            // Arrange
            var valor = 999m;
            var listaOperacoes = new List<OperacaoDto> { new OperacaoDto() };

            Func<ICollection<OperacaoDto>, decimal> acao = (operacoes) =>
            {
                return valor;
            };
            
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoTemplate(listaOperacoes, acao);

            // Assert
            Assert.AreEqual(valor, resultado);
        }

        [TestMethod]
        public void ObterSaldoTemplate_NenhumaOperacao_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoTemplate(new List<OperacaoDto>(), null);

            // Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void ObterSaldoTemplate_PassandoNulo_DeveRetornarZero()
        {
            // Arrange
            var contaRepositorio = new ContaRepositorio(null, null);

            // Act
            var resultado = contaRepositorio.ObterSaldoTemplate(null, null);

            // Assert
            Assert.AreEqual(0, resultado);
        }
    }
}
