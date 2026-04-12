using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
{
    public void Configure(EntityTypeBuilder<Establishment> builder)
    {
        builder.ToTable("Establishments");
        builder.Property(e => e.TradeName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.LegalName).HasMaxLength(200);
        builder.Property(e => e.Cnpj).HasMaxLength(14);
        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.Street).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Number).HasMaxLength(20);
        builder.Property(e => e.Complement).HasMaxLength(100);
        builder.Property(e => e.Neighborhood).HasMaxLength(100);
        builder.Property(e => e.City).HasMaxLength(100).IsRequired();
        builder.Property(e => e.State).HasMaxLength(2).IsRequired();
        builder.Property(e => e.ZipCode).HasMaxLength(8).IsRequired();
        builder.Property(e => e.Latitude).HasColumnType("decimal(10,8)").IsRequired();
        builder.Property(e => e.Longitude).HasColumnType("decimal(11,8)").IsRequired();
        builder.Property(e => e.LogoUrl).HasMaxLength(500);
        builder.Property(e => e.AutoConfirmAppointments).HasDefaultValue(true);
        builder.Property(e => e.PlanType).HasDefaultValue(0);
        builder.Property(e => e.IsApproved).HasDefaultValue(false);

        builder.HasIndex(e => new { e.Latitude, e.Longitude });

        builder.HasMany(e => e.Services)
               .WithOne(s => s.Establishment)
               .HasForeignKey(s => s.EstablishmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Professionals)
               .WithOne(p => p.Establishment)
               .HasForeignKey(p => p.EstablishmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Photos)
               .WithOne(p => p.Establishment)
               .HasForeignKey(p => p.EstablishmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Appointments)
               .WithOne(a => a.Establishment)
               .HasForeignKey(a => a.EstablishmentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
