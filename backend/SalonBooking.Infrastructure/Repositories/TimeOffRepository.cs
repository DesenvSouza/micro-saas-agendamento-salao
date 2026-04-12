using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class TimeOffRepository : BaseRepository<TimeOff>, ITimeOffRepository
{
    public TimeOffRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<TimeOff>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default) =>
        await _dbSet
            .Where(t => t.ProfessionalId == professionalId && t.EndDateTime >= DateTime.UtcNow)
            .OrderBy(t => t.StartDateTime)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TimeOff>> GetOverlappingAsync(
        Guid professionalId,
        DateTime startDateTime,
        DateTime endDateTime,
        CancellationToken cancellationToken = default) =>
        await _dbSet
            .Where(t => t.ProfessionalId == professionalId
                     && t.StartDateTime < endDateTime
                     && t.EndDateTime > startDateTime)
            .ToListAsync(cancellationToken);
}
