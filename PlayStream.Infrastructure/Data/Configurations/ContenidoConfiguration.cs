using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class ContenidoConfiguration : IEntityTypeConfiguration<Contenido>
    {
        public void Configure(EntityTypeBuilder<Contenido> builder)
        {
            builder.ToTable("contenido");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Descripcion).HasMaxLength(500);
            builder.Property(e => e.Categoria).IsRequired().HasMaxLength(50);
        }
    }
}