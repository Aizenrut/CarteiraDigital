using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace CarteiraDigital.Servicos.Testes
{
    [TestClass]
    public class LogServicoTestes
    {
        [TestMethod]
        public void Log_PassandoInformacoes_DeveChamarOPostComAsInformacoes()
        {
            // Arrange
            var excecao = new Exception();
            var mensagem = "Erro";

            Log log = null;

            var logRepositorio = Substitute.For<ILogRepositorio>();
            logRepositorio.When(x => x.Post(Arg.Any<Log>()))
                          .Do(x => log = x.Arg<Log>());

            var logServico = new LogServico(logRepositorio);

            // Act
            logServico.Log(mensagem, excecao);

            // Assert
            logRepositorio.Received(1).Post(Arg.Any<Log>());
            Assert.AreEqual(mensagem, log.Mensagem);
        }
    }
}
