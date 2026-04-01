using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("rol");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.Nombre).HasMaxLength(50);
        }
    }
}