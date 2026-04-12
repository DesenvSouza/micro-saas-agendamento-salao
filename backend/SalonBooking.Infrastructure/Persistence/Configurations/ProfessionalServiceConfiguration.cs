using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class ProfessionalServiceConfiguration : IEntityTypeConfiguration<ProfessionalService>
{
    public void Configure(EntityTypeBuilder<ProfessionalService> builder)
    {
        builder.ToTable("ProfessionalServices");
        builder.HasKey(ps => new { ps.ProfessionalId, ps.ServiceId });

        builder.HasOne(ps => ps.Professional)
               .WithMany(p => p.ProfessionalServices)
               .HasForeignKey(ps => ps.ProfessionalId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.Service)
               .WithMany(s => s.ProfessionalServices)
               .HasForeignKey(ps => ps.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
