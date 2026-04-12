using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetByEstablishmentAsync(
        Guid establishmentId,
        bool onlyActive = true,
        CancellationToken cancellationToken = default);
}
