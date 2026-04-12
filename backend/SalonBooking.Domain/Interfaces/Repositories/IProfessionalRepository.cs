using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IProfessionalRepository : IRepository<Professional>
{
    Task<IEnumerable<Professional>> GetByEstablishmentAsync(
        Guid establishmentId,
        CancellationToken cancellationToken = default);

    Task<Professional?> GetWithScheduleAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);
}
