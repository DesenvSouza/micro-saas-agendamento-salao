using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<IEnumerable<Service>> GetByEstablishmentAsync(
        Guid establishmentId,
        bool onlyActive = true,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(s => s.EstablishmentId == establishmentId);
        if (onlyActive) query = query.Where(s => s.IsActive);
        return await query.OrderBy(s => s.Category).ThenBy(s => s.Name).ToListAsync(cancellationToken);
    }
}
