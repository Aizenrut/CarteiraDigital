using CarteiraDigital.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarteiraDigital.Dados.Configuracoes
{
    class TransferenciaConfiguration : IEntityTypeConfiguration<Transferencia>
    {
        public void Configure(EntityTypeBuilder<Transferencia> builder)
        {
            builder.Property(transferencia => transferencia.Valor)
                   .HasColumnType("MONEY");

            builder.Property(transferencia => transferencia.SaldoAnterior)
                   .HasColumnType("MONEY");

            builder.Property(transferencia => transferencia.Descricao)
                   .HasColumnType("VARCHAR(50)");
        }
    }
}
