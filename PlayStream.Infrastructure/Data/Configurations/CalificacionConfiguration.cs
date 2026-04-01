using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class CalificacionConfiguration : IEntityTypeConfiguration<Calificacion>
    {
        public void Configure(EntityTypeBuilder<Calificacion> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("calificacion");

            builder.HasIndex(e => e.ContenidoId, "FK_Calificacion_Contenido");
            builder.HasIndex(e => e.PerfilId, "FK_Calificacion_Perfil");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.Comentario).HasMaxLength(500);
            builder.Property(e => e.ContenidoId).HasColumnType("int(11)");
            builder.Property(e => e.PerfilId).HasColumnType("int(11)");
            builder.Property(e => e.Puntuacion).HasColumnType("int(11)");

            builder.HasOne(d => d.Contenido).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.ContenidoId)
                .HasConstraintName("FK_Calificacion_Contenido");

            builder.HasOne(d => d.Perfil).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("FK_Calificacion_Perfil");
        }
    }
}