using SalonBooking.Domain.Entities.Base;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Domain.Entities;

public class Notification : Entity
{
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public bool IsRead { get; private set; } = false;
    public Guid? RelatedId { get; private set; }

    public User User { get; private set; } = default!;

    private Notification() { }

    public static Notification Create(
        Guid userId,
        NotificationType type,
        string title,
        string body,
        Guid? relatedId = null)
    {
        return new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Body = body,
            RelatedId = relatedId
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
        SetUpdatedAt();
    }
}
