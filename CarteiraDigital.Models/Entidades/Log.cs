using System;

namespace CarteiraDigital.Models
{
    public class Log : Entidade
    {
        public string Mensagem { get; set; }
        public string StackTrace { get; set; }
        public DateTime Data { get; set; }

        public Log()
        {
        }

        public Log(string mensagem, string stackTrace, DateTime data)
        {
            Mensagem = mensagem;
            StackTrace = stackTrace;
            Data = data;
        }
    }
}
