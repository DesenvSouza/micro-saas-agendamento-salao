using SalonBooking.Domain.Entities.Base;

namespace SalonBooking.Domain.Entities;

public class Professional : Entity
{
    public Guid EstablishmentId { get; private set; }
    public string Name { get; private set; } = default!;
    public string? Bio { get; private set; }
    public string? PhotoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Establishment Establishment { get; private set; } = default!;
    public ICollection<ProfessionalService> ProfessionalServices { get; private set; } = [];
    public ICollection<WorkingSchedule> WorkingSchedules { get; private set; } = [];
    public ICollection<TimeOff> TimeOffs { get; private set; } = [];
    public ICollection<Appointment> Appointments { get; private set; } = [];

    private Professional() { }

    public static Professional Create(Guid establishmentId, string name, string? bio = null)
    {
        return new Professional
        {
            EstablishmentId = establishmentId,
            Name = name.Trim(),
            Bio = bio?.Trim()
        };
    }

    public void Update(string name, string? bio, string? photoUrl)
    {
        Name = name.Trim();
        Bio = bio?.Trim();
        PhotoUrl = photoUrl;
        SetUpdatedAt();
    }

    public void SetPhoto(string photoUrl)
    {
        PhotoUrl = photoUrl;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}
