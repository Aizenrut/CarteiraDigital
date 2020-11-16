using CarteiraDigital.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarteiraDigital.Dados.Configuracoes
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> builder)
        {
            builder.Property(conta => conta.Saldo)
                   .HasColumnType("MONEY");
        }
    }
}
