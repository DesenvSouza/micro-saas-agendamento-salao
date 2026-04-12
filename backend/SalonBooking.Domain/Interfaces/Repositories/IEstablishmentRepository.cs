using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IEstablishmentRepository : IRepository<Establishment>
{
    Task<Establishment?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Establishment>> GetNearbyAsync(
        double latitude,
        double longitude,
        double radiusKm,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
}
