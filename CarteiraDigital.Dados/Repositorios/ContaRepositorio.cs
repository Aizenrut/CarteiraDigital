using CarteiraDigital.Dados.Contexts;
using CarteiraDigital.Models;

namespace CarteiraDigital.Dados.Repositorios
{
    public class ContaRepositorio : Repositorio<Conta>, IContaRepositorio
    {
        public ContaRepositorio(CarteiraDigitalContext context) : base(context)
        {
        }
    }
}
