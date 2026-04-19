namespace SalonBooking.Application.Features.Appointments;

public record CalendarEventDto(
    string Id,
    string Title,
    DateTime Start,
    DateTime End,
    string Color,
    string Status,
    string? ClientName,
    string? ProfessionalName,
    string? ServiceName,
    decimal Price);
