using MediatR;

namespace SalonBooking.Application.Features.Auth.Commands.RegisterEstablishment;

public record RegisterEstablishmentCommand(
    string Email,
    string Password,
    string TradeName,
    string Street,
    string City,
    string State,
    string ZipCode,
    double Latitude,
    double Longitude,
    string? Phone = null) : IRequest<RegisterEstablishmentResponse>;
