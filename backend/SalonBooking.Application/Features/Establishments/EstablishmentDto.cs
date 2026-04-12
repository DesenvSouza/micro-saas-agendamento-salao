using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Establishments;

public record EstablishmentDto(
    Guid Id,
    string TradeName,
    string? LegalName,
    string? Cnpj,
    string? Phone,
    string? Description,
    string Street,
    string? Number,
    string? Complement,
    string? Neighborhood,
    string City,
    string State,
    string ZipCode,
    double Latitude,
    double Longitude,
    string? LogoUrl,
    bool AutoConfirmAppointments,
    SubscriptionPlanType PlanType,
    bool IsApproved,
    DateTime CreatedAt);
