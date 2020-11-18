using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Dados.Servicos;
using CarteiraDigital.Models;
using CarteiraDigital.Servicos.Clients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class CashOutServicoTestes
    {
        [TestMethod]
        public async Task Gerar_CashOutValido_DeveRetornarOCashOutECalcularTaxa()
        {
            // Arrange
            var conta = new Conta { Id = 1 };
            var valor = 10m;
            var descricao = "Teste unitário.";

            CashOut cashOutGerado = null;

            var cashOutRepositorio = Substitute.For<ICashOutRepositorio>();
            cashOutRepositorio.When(x => x.Post(Arg.Any<CashOut>()))
                              .Do(x => cashOutGerado = x.Arg<CashOut>());

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualTaxa().Returns(0.01m);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();
            
            var produtorClient = Substitute.For<IProdutorOperacoesClient>();

            var cashOutServico = new CashOutServico(cashOutRepositorio, operacaoServico, contaServico, configuracaoServico, null, produtorClient);

            // Act
            await cashOutServico.Gerar(new OperacaoUnariaDto(conta.Id, valor, descricao));

            // Assert
            Assert.IsNotNull(cashOutGerado);
            Assert.AreEqual(valor, cashOutGerado.Valor);
            Assert.AreEqual(0.1m, cashOutGerado.ValorTaxa);
            Assert.AreEqual(descricao, cashOutGerado.Descricao);
            Assert.AreEqual(StatusOperacao.Pendente, cashOutGerado.Status);
            Assert.AreNotEqual(default, cashOutGerado.Data);
        }

        [TestMethod]
        public void Efetivar_SaldoSuficiente_DeveSubtraitOValorDaConta()
        {
            // Arrange
            var conta = new Conta
            {
                Id = 1,
                Saldo = 100
            };

            CashOut cashOut = new CashOut
            {
                Id = 1,
                Valor = 100,
                ContaId = conta.Id
            }; ;

            var cashOutRepositorio = Substitute.For<ICashOutRepositorio>();
            cashOutRepositorio.Get(cashOut.Id).Returns(cashOut);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(true);
            contaRepositorio.Get(conta.Id).Returns(conta);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var configuracaoServico = Substitute.For<IConfiguracaoServico>();
            configuracaoServico.ObterPercentualTaxa().Returns(0.01m);

            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var cashOutServico = new CashOutServico(cashOutRepositorio, operacaoServico, contaServico, configuracaoServico, transacaoServico, null);

            // Act
            cashOutServico.Efetivar(new EfetivarOperacaoUnariaDto(cashOut.Id));

            // Assert
            transacaoServico.Received(1).GerarNova();
            transacaoServico.Received(1).Finalizar();
            Assert.AreEqual(StatusOperacao.Efetivada, cashOut.Status);
            Assert.AreEqual(0, conta.Saldo);
            cashOutRepositorio.Received(1).Update(cashOut);
        }

        [TestMethod]
        public void Efetivar_ContaInvalida_DeveLancarExcecaoEPararOProcesso()
        {
            // Arrange
            var conta = new Conta { Id = 1 };

            CashOut cashOut = new CashOut
            {
                Id = 1,
                Valor = 100,
                ContaId = conta.Id
            };

            var cashOutRepositorio = Substitute.For<ICashOutRepositorio>();
            cashOutRepositorio.Get(cashOut.Id).Returns(cashOut);

            var contaRepositorio = Substitute.For<IContaRepositorio>();
            contaRepositorio.Any(conta.Id).Returns(false);

            var contaServico = new ContaServico(contaRepositorio);
            var operacaoServico = new OperacaoServico();

            var transacaoServico = Substitute.For<ITransacaoServico>();
            transacaoServico.GerarNova().Returns(transacaoServico);

            var cashOutServico = new CashOutServico(cashOutRepositorio, operacaoServico, contaServico, null, transacaoServico, null);

            // Act
            cashOutServico.Efetivar(new EfetivarOperacaoUnariaDto(cashOut.Id));

            // Assert
            Assert.AreEqual("A conta informada é inválida!", cashOut.Erro);
            cashOutRepositorio.Received(1).Update(cashOut);
            transacaoServico.Received(0).GerarNova();
            transacaoServico.Received(0).Finalizar();
        }
    }
}
