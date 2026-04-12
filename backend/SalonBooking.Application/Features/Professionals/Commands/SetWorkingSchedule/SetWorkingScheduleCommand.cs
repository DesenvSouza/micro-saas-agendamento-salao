using MediatR;

namespace SalonBooking.Application.Features.Professionals.Commands.SetWorkingSchedule;

public record SetWorkingScheduleCommand(
    Guid ProfessionalId,
    IEnumerable<WorkingScheduleItemInput> Schedules) : IRequest<IEnumerable<WorkingScheduleDto>>;

public record WorkingScheduleItemInput(
    DayOfWeek DayOfWeek,
    bool IsWorkingDay,
    TimeOnly StartTime,
    TimeOnly EndTime,
    TimeOnly? LunchBreakStart = null,
    TimeOnly? LunchBreakEnd = null);
