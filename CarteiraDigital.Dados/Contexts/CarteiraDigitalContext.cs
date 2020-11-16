using CarteiraDigital.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarteiraDigital.Dados.Contexts
{
    public class CarteiraDigitalContext : DbContext
    {
        private static bool tabelasCriadas;

        public DbSet<Conta> Contas { get; set; }
        public DbSet<CashIn> CashIns { get; set; }
        public DbSet<CashOut> CashOuts { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }
        public DbSet<Log> Logs { get; set; }

        public CarteiraDigitalContext(DbContextOptions<CarteiraDigitalContext> options) : base(options)
        {
            Database.EnsureCreated();

            if (!tabelasCriadas)
            {
                var creator = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();

                try
                {
                    creator.CreateTables();
                }
                catch
                {
                }

                tabelasCriadas = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarteiraDigitalContext).Assembly);
        }
    }
}
