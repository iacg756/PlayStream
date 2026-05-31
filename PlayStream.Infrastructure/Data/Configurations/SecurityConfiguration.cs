using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayStream.Core.Entities;
using PlayStream.Core.Enum;

namespace PlayStream.Infrastructure.Data.Configurations
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.ToTable("security");

            builder.Property(e => e.Id).HasColumnType("int(11)");
            builder.Property(e => e.Login).HasMaxLength(50).IsUnicode(false);
            builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
            builder.Property(e => e.Password).HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)System.Enum.Parse(typeof(RoleType), x)
                );
        }
    }
}