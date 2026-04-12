namespace SalonBooking.Domain.ValueObjects;

public record Address(
    string Street,
    string? Number,
    string? Complement,
    string? Neighborhood,
    string City,
    string State,
    string ZipCode);
