using MediatR;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalSchedule;

public class GetProfessionalScheduleQueryHandler
    : IRequestHandler<GetProfessionalScheduleQuery, IEnumerable<WorkingScheduleDto>>
{
    private readonly IWorkingScheduleRepository _scheduleRepository;

    public GetProfessionalScheduleQueryHandler(IWorkingScheduleRepository scheduleRepository) =>
        _scheduleRepository = scheduleRepository;

    public async Task<IEnumerable<WorkingScheduleDto>> Handle(
        GetProfessionalScheduleQuery request,
        CancellationToken cancellationToken)
    {
        var schedules = await _scheduleRepository.GetByProfessionalAsync(
            request.ProfessionalId, cancellationToken);

        return schedules.OrderBy(s => s.DayOfWeek).Select(s => s.ToDto());
    }
}
