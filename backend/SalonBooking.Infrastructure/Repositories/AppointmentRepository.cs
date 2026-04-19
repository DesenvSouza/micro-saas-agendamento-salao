using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetByClientAsync(
        Guid clientId,
        bool onlyUpcoming = false,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(a => a.Establishment)
            .Include(a => a.Professional)
            .Include(a => a.Service)
            .Where(a => a.ClientId == clientId);

        if (onlyUpcoming)
            query = query.Where(a => a.StartTime >= DateTime.UtcNow
                && (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed));

        return await query.OrderByDescending(a => a.StartTime).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByEstablishmentAsync(
        Guid establishmentId,
        DateTime? from = null,
        DateTime? to = null,
        AppointmentStatus? status = null,
        Guid? professionalId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(a => a.Client)
            .Include(a => a.Professional)
            .Include(a => a.Service)
            .Where(a => a.EstablishmentId == establishmentId)
            .AsQueryable();

        if (from.HasValue) query = query.Where(a => a.StartTime >= from.Value);
        if (to.HasValue) query = query.Where(a => a.StartTime <= to.Value);
        if (status.HasValue) query = query.Where(a => a.Status == status.Value);
        if (professionalId.HasValue) query = query.Where(a => a.ProfessionalId == professionalId.Value);

        return await query.OrderBy(a => a.StartTime).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetForProfessionalOnDateAsync(
        Guid professionalId,
        DateTime dateUtcStart,
        DateTime dateUtcEnd,
        IEnumerable<AppointmentStatus> statuses,
        CancellationToken cancellationToken = default)
    {
        var statusList = statuses.ToList();
        return await _dbSet
            .Where(a => a.ProfessionalId == professionalId
                     && a.StartTime < dateUtcEnd
                     && a.EndTime > dateUtcStart
                     && statusList.Contains(a.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasConflictAsync(
        Guid professionalId,
        DateTime startTime,
        DateTime endTime,
        Guid? excludeAppointmentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(a =>
            a.ProfessionalId == professionalId &&
            a.StartTime < endTime &&
            a.EndTime > startTime &&
            (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed));

        if (excludeAppointmentId.HasValue)
            query = query.Where(a => a.Id != excludeAppointmentId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<Appointment?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbSet
            .Include(a => a.Client)
            .Include(a => a.Establishment)
            .Include(a => a.Professional)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
}
