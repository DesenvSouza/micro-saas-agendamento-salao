using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface ITimeOffRepository : IRepository<TimeOff>
{
    Task<IEnumerable<TimeOff>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TimeOff>> GetOverlappingAsync(
        Guid professionalId,
        DateTime startDateTime,
        DateTime endDateTime,
        CancellationToken cancellationToken = default);
}
