using CarteiraDigital.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarteiraDigital.Dados.Configuracoes
{
    public class CashInConfiguration : IEntityTypeConfiguration<CashIn>
    {
        public void Configure(EntityTypeBuilder<CashIn> builder)
        {
            builder.Property(cashIn => cashIn.Valor)
                   .HasColumnType("MONEY");

            builder.Property(cashIn => cashIn.SaldoAnterior)
                   .HasColumnType("MONEY");

            builder.Property(cashIn => cashIn.Descricao)
                   .HasColumnType("VARCHAR(50)");
        }
    }
}
