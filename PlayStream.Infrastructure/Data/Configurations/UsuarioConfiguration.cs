using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("usuario");

            builder.HasIndex(e => e.Correo, "Correo").IsUnique();
            builder.HasIndex(e => e.RolId, "FK_Usuario_Rol");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.Correo).HasMaxLength(150);
            builder.Property(e => e.FechaRegistro).HasColumnType("datetime");
            builder.Property(e => e.Nombre).HasMaxLength(100);
            builder.Property(e => e.RolId).HasColumnType("int(11)");

            builder.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        }
    }
}