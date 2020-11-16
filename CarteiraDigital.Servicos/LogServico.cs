using CarteiraDigital.Dados.Repositorios;
using CarteiraDigital.Models;
using System;

namespace CarteiraDigital.Servicos
{
    public class LogServico : ILogServico
    {
        private readonly ILogRepositorio logRepositorio;

        public LogServico(ILogRepositorio logRepositorio)
        {
            this.logRepositorio = logRepositorio;
        }

        public void Log(string mensagem, Exception excecao)
        {
            logRepositorio.Post(new Log(mensagem, excecao.StackTrace, DateTime.Now));
        }
    }
}
