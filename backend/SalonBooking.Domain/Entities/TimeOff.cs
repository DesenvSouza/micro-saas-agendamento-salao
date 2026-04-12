using SalonBooking.Domain.Entities.Base;

namespace SalonBooking.Domain.Entities;

public class TimeOff : Entity
{
    public Guid ProfessionalId { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public string? Reason { get; private set; }

    public Professional Professional { get; private set; } = default!;

    private TimeOff() { }

    public static TimeOff Create(
        Guid professionalId,
        DateTime startDateTime,
        DateTime endDateTime,
        string? reason = null)
    {
        return new TimeOff
        {
            ProfessionalId = professionalId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            Reason = reason?.Trim()
        };
    }

    public bool OverlapsWith(DateTime start, DateTime end) =>
        start < EndDateTime && end > StartDateTime;
}
