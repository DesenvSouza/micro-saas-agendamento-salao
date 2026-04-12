namespace SalonBooking.Application.Features.Professionals;

public record ProfessionalDto(
    Guid Id,
    Guid EstablishmentId,
    string Name,
    string? Bio,
    string? PhotoUrl,
    bool IsActive,
    DateTime CreatedAt,
    IEnumerable<ProfessionalServiceItemDto> Services);

public record ProfessionalServiceItemDto(Guid ServiceId, string ServiceName, int DurationMinutes, decimal Price);

public record WorkingScheduleDto(
    Guid Id,
    DayOfWeek DayOfWeek,
    string DayName,
    bool IsWorkingDay,
    TimeOnly StartTime,
    TimeOnly EndTime,
    TimeOnly? LunchBreakStart,
    TimeOnly? LunchBreakEnd);

public record TimeOffDto(
    Guid Id,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Reason);
