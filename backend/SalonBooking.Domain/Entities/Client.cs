using SalonBooking.Domain.Enums;
using SalonBooking.Domain.ValueObjects;

namespace SalonBooking.Domain.Entities;

public class Client : User
{
    public string FullName { get; private set; } = default!;
    public string? Phone { get; private set; }
    public string? PhotoUrl { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    public ICollection<Appointment> Appointments { get; private set; } = [];

    private Client() { }

    public static Client Create(string email, string passwordHash, string fullName, string? phone = null)
    {
        return new Client
        {
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            Phone = phone?.Trim(),
            Role = UserRole.Client,
            EmailVerificationToken = Guid.NewGuid().ToString("N")
        };
    }

    public void UpdateLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        SetUpdatedAt();
    }

    public void UpdateProfile(string fullName, string? phone, string? photoUrl)
    {
        FullName = fullName.Trim();
        Phone = phone?.Trim();
        PhotoUrl = photoUrl;
        SetUpdatedAt();
    }

    public Coordinates? GetCoordinates() =>
        Latitude.HasValue && Longitude.HasValue
            ? new Coordinates(Latitude.Value, Longitude.Value)
            : null;
}
