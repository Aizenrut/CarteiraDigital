using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public class CashOutRepositorio : Repositorio<CashOut>, ICashOutRepositorio
    {
        public CashOutRepositorio(CarteiraDigitalContext context) : base(context)
        {
        }
    }
}
