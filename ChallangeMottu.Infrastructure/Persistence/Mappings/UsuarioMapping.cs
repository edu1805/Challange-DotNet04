using ChallangeMottu.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallangeMottu.Infrastructure.Persistence.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("T_USUARIOS-MOTTU"); // nome da tabela no Oracle

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("ID")
            .HasColumnType("RAW(16)")
            .IsRequired()
            .HasDefaultValueSql("SYS_GUID()");

        builder.Property(u => u.Nome)
            .HasColumnName("NOME")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(u => u.Moto)
            .WithMany()
            .HasForeignKey(u => u.MotoId)
            .IsRequired(false); // moto opcional
    }
}