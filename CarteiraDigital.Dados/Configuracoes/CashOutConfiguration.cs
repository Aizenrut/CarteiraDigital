using CarteiraDigital.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarteiraDigital.Dados.Configuracoes
{
    public class CashOutConfiguration : IEntityTypeConfiguration<CashOut>
    {
        public void Configure(EntityTypeBuilder<CashOut> builder)
        {
            builder.Property(cashOut => cashOut.Valor)
                   .HasColumnType("MONEY");

            builder.Property(cashOut => cashOut.ValorTaxa)
                   .HasColumnType("MONEY");

            builder.Property(cashOut => cashOut.SaldoAnterior)
                   .HasColumnType("MONEY");

            builder.Property(cashOut => cashOut.Descricao)
                   .HasColumnType("VARCHAR(50)");
        }
    }
}
