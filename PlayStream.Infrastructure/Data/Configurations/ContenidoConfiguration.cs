using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class ContenidoConfiguration : IEntityTypeConfiguration<Contenido>
    {
        public void Configure(EntityTypeBuilder<Contenido> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("contenido");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.AnioLanzamiento).HasColumnType("int(11)");
            builder.Property(e => e.Categoria).HasMaxLength(50);
            builder.Property(e => e.Descripcion).HasMaxLength(500);
            builder.Property(e => e.Titulo).HasMaxLength(200);
        }
    }
}