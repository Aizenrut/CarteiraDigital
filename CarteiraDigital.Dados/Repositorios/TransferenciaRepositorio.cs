using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public class TransferenciaRepositorio : Repositorio<Transferencia>, ITransferenciaRepositorio
    {
        public TransferenciaRepositorio(CarteiraDigitalContext context) : base(context)
        {
        }
    }
}
