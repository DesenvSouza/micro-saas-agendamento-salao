using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class EstablishmentPhotoConfiguration : IEntityTypeConfiguration<EstablishmentPhoto>
{
    public void Configure(EntityTypeBuilder<EstablishmentPhoto> builder)
    {
        builder.ToTable("EstablishmentPhotos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(p => p.Url).HasMaxLength(500).IsRequired();
        builder.Property(p => p.DisplayOrder).HasDefaultValue(0);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
    }
}
