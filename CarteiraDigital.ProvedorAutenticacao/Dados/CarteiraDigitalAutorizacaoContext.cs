using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarteiraDigital.ProvedorAutenticacao.Models;

namespace CarteiraDigital.ProvedorAutenticacao.Dados
{
    public class CarteiraDigitalAutorizacaoContext : IdentityDbContext<Usuario>
    {
        public CarteiraDigitalAutorizacaoContext(DbContextOptions<CarteiraDigitalAutorizacaoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
