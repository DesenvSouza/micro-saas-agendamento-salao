using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(r => r.Token).HasMaxLength(500).IsRequired();
        builder.HasIndex(r => r.Token).IsUnique();
        builder.Property(r => r.ExpiresAt).HasColumnType("datetime2").IsRequired();
        builder.Property(r => r.IsRevoked).HasDefaultValue(false);
        builder.Property(r => r.ReplacedByToken).HasMaxLength(500);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
    }
}
