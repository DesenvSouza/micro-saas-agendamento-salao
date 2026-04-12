using SalonBooking.Domain.Entities.Base;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Domain.Entities;

public class Service : Entity
{
    public Guid EstablishmentId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public ServiceCategory Category { get; private set; }
    public int DurationMinutes { get; private set; }
    public int SlotIntervalMinutes { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public bool IsActive { get; private set; } = true;

    public Establishment Establishment { get; private set; } = default!;
    public ICollection<ProfessionalService> ProfessionalServices { get; private set; } = [];

    private Service() { }

    public static Service Create(
        Guid establishmentId,
        string name,
        ServiceCategory category,
        int durationMinutes,
        decimal price,
        string? description = null,
        int? slotIntervalMinutes = null)
    {
        return new Service
        {
            EstablishmentId = establishmentId,
            Name = name.Trim(),
            Description = description?.Trim(),
            Category = category,
            DurationMinutes = durationMinutes,
            SlotIntervalMinutes = slotIntervalMinutes ?? durationMinutes,
            Price = price
        };
    }

    public void Update(
        string name,
        ServiceCategory category,
        int durationMinutes,
        decimal price,
        string? description = null,
        int? slotIntervalMinutes = null)
    {
        Name = name.Trim();
        Description = description?.Trim();
        Category = category;
        DurationMinutes = durationMinutes;
        SlotIntervalMinutes = slotIntervalMinutes ?? durationMinutes;
        Price = price;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }
}
