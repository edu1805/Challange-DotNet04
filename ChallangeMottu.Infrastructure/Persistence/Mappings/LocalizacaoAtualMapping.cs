using ChallangeMottu.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallangeMottu.Infrastructure.Persistence.Mappings;

public class LocalizacaoAtualMapping : IEntityTypeConfiguration<LocalizacaoAtual>
{
    public void Configure(EntityTypeBuilder<LocalizacaoAtual> builder)
    {
        builder.ToTable("T_LOCALIZACAO_ATUAL-MOTTU");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasColumnName("ID")
            .HasColumnType("RAW(16)")
            .IsRequired()
            .HasDefaultValueSql("SYS_GUID()");

        builder.Property(l => l.CoordenadaX).IsRequired();
        builder.Property(l => l.CoordenadaY).IsRequired();
        builder.Property(l => l.DataHoraAtualizacao).IsRequired();

        builder.HasOne(l => l.Moto)
            .WithMany()
            .HasForeignKey(l => l.MotoId);
    }
}