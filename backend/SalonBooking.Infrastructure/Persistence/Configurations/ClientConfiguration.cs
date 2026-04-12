using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.Property(c => c.FullName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.PhotoUrl).HasMaxLength(500);
        builder.Property(c => c.Latitude).HasColumnType("decimal(10,8)");
        builder.Property(c => c.Longitude).HasColumnType("decimal(11,8)");

        builder.HasMany(c => c.Appointments)
               .WithOne(a => a.Client)
               .HasForeignKey(a => a.ClientId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
