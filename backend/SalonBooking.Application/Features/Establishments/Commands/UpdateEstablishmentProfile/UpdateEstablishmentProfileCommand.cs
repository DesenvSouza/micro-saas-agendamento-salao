using MediatR;

namespace SalonBooking.Application.Features.Establishments.Commands.UpdateEstablishmentProfile;

public record UpdateEstablishmentProfileCommand(
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
    bool AutoConfirmAppointments) : IRequest<EstablishmentDto>;
