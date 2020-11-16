using System;

namespace CarteiraDigital.Servicos
{
    public interface ILogServico
    {
        void Log(string mensagem, Exception excecao);
    }
}
