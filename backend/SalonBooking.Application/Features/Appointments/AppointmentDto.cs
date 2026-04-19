using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Appointments;

public record AppointmentDto(
    Guid Id,
    Guid ClientId,
    string? ClientName,
    string? ClientPhone,
    Guid EstablishmentId,
    string? EstablishmentName,
    Guid ProfessionalId,
    string? ProfessionalName,
    Guid ServiceId,
    string? ServiceName,
    DateTime StartTime,
    DateTime EndTime,
    AppointmentStatus Status,
    string StatusName,
    decimal Price,
    string Currency,
    string? ClientNotes,
    string? EstablishmentNotes,
    DateTime CreatedAt);
