using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public class LogRepositorio : Repositorio<Log>, ILogRepositorio
    {
        public LogRepositorio(CarteiraDigitalContext context) : base(context)
        {
        }
    }
}
