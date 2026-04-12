using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("Professionals");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Bio).HasMaxLength(1000);
        builder.Property(p => p.PhotoUrl).HasMaxLength(500);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(p => p.WorkingSchedules)
               .WithOne(ws => ws.Professional)
               .HasForeignKey(ws => ws.ProfessionalId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.TimeOffs)
               .WithOne(t => t.Professional)
               .HasForeignKey(t => t.ProfessionalId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Appointments)
               .WithOne(a => a.Professional)
               .HasForeignKey(a => a.ProfessionalId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
