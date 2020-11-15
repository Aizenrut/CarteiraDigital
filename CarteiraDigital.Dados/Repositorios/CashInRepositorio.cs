using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public class CashInRepositorio : Repositorio<CashIn>, ICashInRepositorio
    {
        public CashInRepositorio(CarteiraDigitalContext context) : base(context)
        {
        }
    }
}
