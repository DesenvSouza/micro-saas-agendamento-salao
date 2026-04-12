using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.SetWorkingSchedule;

public class SetWorkingScheduleCommandHandler
    : IRequestHandler<SetWorkingScheduleCommand, IEnumerable<WorkingScheduleDto>>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IWorkingScheduleRepository _scheduleRepository;
    private readonly ICurrentUserService _currentUserService;

    public SetWorkingScheduleCommandHandler(
        IProfessionalRepository professionalRepository,
        IWorkingScheduleRepository scheduleRepository,
        ICurrentUserService currentUserService)
    {
        _professionalRepository = professionalRepository;
        _scheduleRepository = scheduleRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<WorkingScheduleDto>> Handle(
        SetWorkingScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Professional), request.ProfessionalId);

        if (professional.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException("Profissional não pertence ao seu estabelecimento.");

        // Replace all schedules atomically
        await _scheduleRepository.RemoveAllByProfessionalAsync(request.ProfessionalId, cancellationToken);

        var newSchedules = new List<WorkingSchedule>();
        foreach (var item in request.Schedules)
        {
            var schedule = WorkingSchedule.Create(
                request.ProfessionalId,
                item.DayOfWeek,
                item.StartTime,
                item.EndTime,
                item.IsWorkingDay,
                item.LunchBreakStart,
                item.LunchBreakEnd);

            await _scheduleRepository.AddAsync(schedule, cancellationToken);
            newSchedules.Add(schedule);
        }

        return newSchedules.OrderBy(s => s.DayOfWeek).Select(s => s.ToDto());
    }
}
