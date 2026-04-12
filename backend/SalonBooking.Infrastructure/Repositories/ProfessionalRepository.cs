using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class ProfessionalRepository : BaseRepository<Professional>, IProfessionalRepository
{
    public ProfessionalRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Professional>> GetByEstablishmentAsync(
        Guid establishmentId,
        CancellationToken cancellationToken = default) =>
        await _dbSet
            .Where(p => p.EstablishmentId == establishmentId && p.IsActive)
            .Include(p => p.ProfessionalServices)
                .ThenInclude(ps => ps.Service)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

    public async Task<Professional?> GetWithScheduleAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default) =>
        await _dbSet
            .Include(p => p.WorkingSchedules)
            .Include(p => p.TimeOffs)
            .FirstOrDefaultAsync(p => p.Id == professionalId, cancellationToken);
}
