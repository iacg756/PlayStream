using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable("perfil");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.NombrePerfil).IsRequired().HasMaxLength(50);
            builder.Property(e => e.AvatarUrl).HasMaxLength(255);
        }
    }
}