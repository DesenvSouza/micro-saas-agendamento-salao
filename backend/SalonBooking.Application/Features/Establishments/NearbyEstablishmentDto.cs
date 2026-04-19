namespace SalonBooking.Application.Features.Establishments;

public record NearbyEstablishmentDto(
    Guid Id,
    string TradeName,
    string? Description,
    string Street,
    string? Number,
    string City,
    string State,
    string ZipCode,
    double Latitude,
    double Longitude,
    string? LogoUrl,
    double DistanceKm);
