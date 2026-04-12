using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Infrastructure.Persistence;

public class SalonBookingDbContext : DbContext
{
    public SalonBookingDbContext(DbContextOptions<SalonBookingDbContext> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Establishment> Establishments => Set<Establishment>();
    public DbSet<EstablishmentPhoto> EstablishmentPhotos => Set<EstablishmentPhoto>();
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ProfessionalService> ProfessionalServices => Set<ProfessionalService>();
    public DbSet<WorkingSchedule> WorkingSchedules => Set<WorkingSchedule>();
    public DbSet<TimeOff> TimeOffs => Set<TimeOff>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalonBookingDbContext).Assembly);
    }
}
