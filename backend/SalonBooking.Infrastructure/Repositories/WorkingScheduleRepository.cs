using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class WorkingScheduleRepository : BaseRepository<WorkingSchedule>, IWorkingScheduleRepository
{
    public WorkingScheduleRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<WorkingSchedule?> GetForDayAsync(
        Guid professionalId,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken = default) =>
        await _dbSet.FirstOrDefaultAsync(
            ws => ws.ProfessionalId == professionalId && ws.DayOfWeek == dayOfWeek,
            cancellationToken);

    public async Task<IEnumerable<WorkingSchedule>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default) =>
        await _dbSet
            .Where(ws => ws.ProfessionalId == professionalId)
            .OrderBy(ws => ws.DayOfWeek)
            .ToListAsync(cancellationToken);

    public async Task RemoveAllByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default)
    {
        var schedules = await _dbSet
            .Where(ws => ws.ProfessionalId == professionalId)
            .ToListAsync(cancellationToken);
        _dbSet.RemoveRange(schedules);
    }
}
