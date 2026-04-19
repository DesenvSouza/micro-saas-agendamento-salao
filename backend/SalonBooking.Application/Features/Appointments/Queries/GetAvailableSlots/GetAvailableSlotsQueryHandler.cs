using MediatR;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Queries.GetAvailableSlots;

public class GetAvailableSlotsQueryHandler
    : IRequestHandler<GetAvailableSlotsQuery, IEnumerable<DateTime>>
{
    private readonly IWorkingScheduleRepository _scheduleRepository;
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceRepository _serviceRepository;

    public GetAvailableSlotsQueryHandler(
        IWorkingScheduleRepository scheduleRepository,
        ITimeOffRepository timeOffRepository,
        IAppointmentRepository appointmentRepository,
        IServiceRepository serviceRepository)
    {
        _scheduleRepository = scheduleRepository;
        _timeOffRepository = timeOffRepository;
        _appointmentRepository = appointmentRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<DateTime>> Handle(
        GetAvailableSlotsQuery request,
        CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Service), request.ServiceId);

        var schedule = await _scheduleRepository.GetForDayAsync(
            request.ProfessionalId, request.Date.DayOfWeek, cancellationToken);

        if (schedule is null || !schedule.IsWorkingDay)
            return [];

        var dayStart = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var workStart = dayStart.Add(schedule.StartTime.ToTimeSpan());
        var workEnd = dayStart.Add(schedule.EndTime.ToTimeSpan());

        var blockers = new List<(DateTime Start, DateTime End)>();

        // Lunch break
        if (schedule.LunchBreakStart.HasValue && schedule.LunchBreakEnd.HasValue)
            blockers.Add((
                dayStart.Add(schedule.LunchBreakStart.Value.ToTimeSpan()),
                dayStart.Add(schedule.LunchBreakEnd.Value.ToTimeSpan())));

        // Time-offs overlapping the workday
        var timeOffs = await _timeOffRepository.GetOverlappingAsync(
            request.ProfessionalId, workStart, workEnd, cancellationToken);
        foreach (var to in timeOffs)
            blockers.Add((to.StartDateTime, to.EndDateTime));

        // Existing appointments (Pending/Confirmed) on this day
        var existing = await _appointmentRepository.GetForProfessionalOnDateAsync(
            request.ProfessionalId,
            workStart,
            workEnd,
            [AppointmentStatus.Pending, AppointmentStatus.Confirmed],
            cancellationToken);
        foreach (var appt in existing)
            blockers.Add((appt.StartTime, appt.EndTime));

        // Generate candidate slots
        var duration = TimeSpan.FromMinutes(service.DurationMinutes);
        var interval = TimeSpan.FromMinutes(service.SlotIntervalMinutes);
        var slots = new List<DateTime>();
        var current = workStart;

        while (current + duration <= workEnd)
        {
            var candidateEnd = current + duration;
            bool blocked = blockers.Any(b => current < b.End && candidateEnd > b.Start);
            if (!blocked)
                slots.Add(current);
            current += interval;
        }

        return slots;
    }
}
