using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence.Configurations;

public class WorkingScheduleConfiguration : IEntityTypeConfiguration<WorkingSchedule>
{
    public void Configure(EntityTypeBuilder<WorkingSchedule> builder)
    {
        builder.ToTable("WorkingSchedules");
        builder.HasKey(ws => ws.Id);
        builder.Property(ws => ws.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(ws => ws.DayOfWeek).IsRequired();
        builder.Property(ws => ws.StartTime).HasColumnType("time").IsRequired();
        builder.Property(ws => ws.EndTime).HasColumnType("time").IsRequired();
        builder.Property(ws => ws.LunchBreakStart).HasColumnType("time");
        builder.Property(ws => ws.LunchBreakEnd).HasColumnType("time");
        builder.Property(ws => ws.IsWorkingDay).HasDefaultValue(true);
        builder.Property(ws => ws.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(ws => ws.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(ws => new { ws.ProfessionalId, ws.DayOfWeek }).IsUnique();
    }
}
