using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IWorkingScheduleRepository : IRepository<WorkingSchedule>
{
    Task<WorkingSchedule?> GetForDayAsync(
        Guid professionalId,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<WorkingSchedule>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);

    Task RemoveAllByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);
}
