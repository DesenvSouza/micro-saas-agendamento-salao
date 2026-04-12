using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class TimeOffConfiguration : IEntityTypeConfiguration<TimeOff>
{
    public void Configure(EntityTypeBuilder<TimeOff> builder)
    {
        builder.ToTable("TimeOffs");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(t => t.StartDateTime).HasColumnType("datetime2").IsRequired();
        builder.Property(t => t.EndDateTime).HasColumnType("datetime2").IsRequired();
        builder.Property(t => t.Reason).HasMaxLength(500);
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(t => new { t.ProfessionalId, t.StartDateTime, t.EndDateTime });
    }
}
