using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(a => a.StartTime).HasColumnType("datetime2").IsRequired();
        builder.Property(a => a.EndTime).HasColumnType("datetime2").IsRequired();
        builder.Property(a => a.Status).IsRequired();
        builder.Property(a => a.Price).HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(a => a.Currency).HasMaxLength(3).HasDefaultValue("BRL");
        builder.Property(a => a.ClientNotes).HasMaxLength(1000);
        builder.Property(a => a.EstablishmentNotes).HasMaxLength(1000);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(a => a.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        // Critical indexes for the slots algorithm and calendar views
        builder.HasIndex(a => new { a.ProfessionalId, a.StartTime, a.Status })
               .HasDatabaseName("IX_Appointments_Professional_Time_Status");
        builder.HasIndex(a => new { a.EstablishmentId, a.StartTime })
               .HasDatabaseName("IX_Appointments_Establishment_Time");
        builder.HasIndex(a => new { a.ClientId, a.StartTime })
               .HasDatabaseName("IX_Appointments_Client_Time");

        builder.HasOne(a => a.Service)
               .WithMany()
               .HasForeignKey(a => a.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
