using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("perfil");

            builder.HasIndex(e => e.UsuarioId, "FK_Perfil_Usuario");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.AvatarUrl).HasMaxLength(255);
            builder.Property(e => e.NombrePerfil).HasMaxLength(50);
            builder.Property(e => e.UsuarioId).HasColumnType("int(11)");

            builder.HasOne(d => d.Usuario).WithMany(p => p.Perfils)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Perfil_Usuario");
        }
    }
}