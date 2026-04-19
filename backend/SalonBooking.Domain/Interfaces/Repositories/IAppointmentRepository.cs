using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByClientAsync(
        Guid clientId,
        bool onlyUpcoming = false,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Appointment>> GetByEstablishmentAsync(
        Guid establishmentId,
        DateTime? from = null,
        DateTime? to = null,
        AppointmentStatus? status = null,
        Guid? professionalId = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Appointment>> GetForProfessionalOnDateAsync(
        Guid professionalId,
        DateTime dateUtcStart,
        DateTime dateUtcEnd,
        IEnumerable<AppointmentStatus> statuses,
        CancellationToken cancellationToken = default);

    Task<bool> HasConflictAsync(
        Guid professionalId,
        DateTime startTime,
        DateTime endTime,
        Guid? excludeAppointmentId = null,
        CancellationToken cancellationToken = default);

    Task<Appointment?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
