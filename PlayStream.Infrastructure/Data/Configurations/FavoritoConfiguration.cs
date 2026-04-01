using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class FavoritoConfiguration : IEntityTypeConfiguration<Favorito>
    {
        public void Configure(EntityTypeBuilder<Favorito> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("favorito");

            builder.HasIndex(e => e.ContenidoId, "FK_Favorito_Contenido");
            builder.HasIndex(e => e.PerfilId, "FK_Favorito_Perfil");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.ContenidoId).HasColumnType("int(11)");
            builder.Property(e => e.PerfilId).HasColumnType("int(11)");

            builder.HasOne(d => d.Contenido).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.ContenidoId)
                .HasConstraintName("FK_Favorito_Contenido");

            builder.HasOne(d => d.Perfil).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("FK_Favorito_Perfil");
        }
    }
}