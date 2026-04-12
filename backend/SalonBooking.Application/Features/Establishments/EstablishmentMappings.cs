using SalonBooking.Domain.Entities;

namespace SalonBooking.Application.Features.Establishments;

public static class EstablishmentMappings
{
    public static EstablishmentDto ToDto(this Establishment e) => new(
        e.Id,
        e.TradeName,
        e.LegalName,
        e.Cnpj,
        e.Phone,
        e.Description,
        e.Street,
        e.Number,
        e.Complement,
        e.Neighborhood,
        e.City,
        e.State,
        e.ZipCode,
        e.Latitude,
        e.Longitude,
        e.LogoUrl,
        e.AutoConfirmAppointments,
        e.PlanType,
        e.IsApproved,
        e.CreatedAt);
}
