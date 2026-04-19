using SalonBooking.Domain.Entities;

namespace SalonBooking.Application.Features.Appointments;

public static class AppointmentMappings
{
    public static AppointmentDto ToDto(this Appointment a) => new(
        a.Id,
        a.ClientId,
        a.Client?.FullName,
        a.Client?.Phone,
        a.EstablishmentId,
        a.Establishment?.TradeName,
        a.ProfessionalId,
        a.Professional?.Name,
        a.ServiceId,
        a.Service?.Name,
        a.StartTime,
        a.EndTime,
        a.Status,
        a.Status.ToString(),
        a.Price,
        a.Currency,
        a.ClientNotes,
        a.EstablishmentNotes,
        a.CreatedAt);
}
