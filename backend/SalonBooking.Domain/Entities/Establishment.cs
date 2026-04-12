using SalonBooking.Domain.Enums;
using SalonBooking.Domain.ValueObjects;

namespace SalonBooking.Domain.Entities;

public class Establishment : User
{
    public string TradeName { get; private set; } = default!;
    public string? LegalName { get; private set; }
    public string? Cnpj { get; private set; }
    public string? Phone { get; private set; }
    public string? Description { get; private set; }
    public string Street { get; private set; } = default!;
    public string? Number { get; private set; }
    public string? Complement { get; private set; }
    public string? Neighborhood { get; private set; }
    public string City { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string ZipCode { get; private set; } = default!;
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool AutoConfirmAppointments { get; private set; } = true;
    public SubscriptionPlanType PlanType { get; private set; } = SubscriptionPlanType.Free;
    public DateTime? PlanExpiresAt { get; private set; }
    public bool IsApproved { get; private set; } = false;

    public ICollection<Service> Services { get; private set; } = [];
    public ICollection<Professional> Professionals { get; private set; } = [];
    public ICollection<EstablishmentPhoto> Photos { get; private set; } = [];
    public ICollection<Appointment> Appointments { get; private set; } = [];

    private Establishment() { }

    public static Establishment Create(
        string email,
        string passwordHash,
        string tradeName,
        string street,
        string city,
        string state,
        string zipCode,
        double latitude,
        double longitude)
    {
        return new Establishment
        {
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            TradeName = tradeName.Trim(),
            Street = street.Trim(),
            City = city.Trim(),
            State = state.Trim().ToUpper(),
            ZipCode = zipCode.Trim().Replace("-", ""),
            Latitude = latitude,
            Longitude = longitude,
            Role = UserRole.Establishment,
            EmailVerificationToken = Guid.NewGuid().ToString("N")
        };
    }

    public void UpdateProfile(
        string tradeName,
        string? legalName,
        string? cnpj,
        string? phone,
        string? description,
        string street,
        string? number,
        string? complement,
        string? neighborhood,
        string city,
        string state,
        string zipCode,
        double latitude,
        double longitude,
        bool autoConfirm)
    {
        TradeName = tradeName.Trim();
        LegalName = legalName?.Trim();
        Cnpj = cnpj?.Trim().Replace(".", "").Replace("/", "").Replace("-", "");
        Phone = phone?.Trim();
        Description = description?.Trim();
        Street = street.Trim();
        Number = number?.Trim();
        Complement = complement?.Trim();
        Neighborhood = neighborhood?.Trim();
        City = city.Trim();
        State = state.Trim().ToUpper();
        ZipCode = zipCode.Trim().Replace("-", "");
        Latitude = latitude;
        Longitude = longitude;
        AutoConfirmAppointments = autoConfirm;
        SetUpdatedAt();
    }

    public void SetLogo(string logoUrl)
    {
        LogoUrl = logoUrl;
        SetUpdatedAt();
    }

    public void Approve()
    {
        IsApproved = true;
        SetUpdatedAt();
    }

    public void SetPlan(SubscriptionPlanType plan, DateTime expiresAt)
    {
        PlanType = plan;
        PlanExpiresAt = expiresAt;
        SetUpdatedAt();
    }

    public Coordinates GetCoordinates() => new(Latitude, Longitude);
    public Address GetAddress() => new(Street, Number, Complement, Neighborhood, City, State, ZipCode);
}
