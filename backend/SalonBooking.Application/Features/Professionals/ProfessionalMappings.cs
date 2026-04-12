using SalonBooking.Domain.Entities;

namespace SalonBooking.Application.Features.Professionals;

public static class ProfessionalMappings
{
    public static ProfessionalDto ToDto(this Professional professional) => new(
        professional.Id,
        professional.EstablishmentId,
        professional.Name,
        professional.Bio,
        professional.PhotoUrl,
        professional.IsActive,
        professional.CreatedAt,
        professional.ProfessionalServices.Select(ps => new ProfessionalServiceItemDto(
            ps.ServiceId,
            ps.Service?.Name ?? string.Empty,
            ps.Service?.DurationMinutes ?? 0,
            ps.Service?.Price ?? 0)));

    public static WorkingScheduleDto ToDto(this WorkingSchedule ws) => new(
        ws.Id,
        ws.DayOfWeek,
        ws.DayOfWeek.ToString(),
        ws.IsWorkingDay,
        ws.StartTime,
        ws.EndTime,
        ws.LunchBreakStart,
        ws.LunchBreakEnd);

    public static TimeOffDto ToDto(this TimeOff timeOff) => new(
        timeOff.Id,
        timeOff.StartDateTime,
        timeOff.EndDateTime,
        timeOff.Reason);
}
