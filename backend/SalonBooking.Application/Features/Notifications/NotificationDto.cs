using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Notifications;

public record NotificationDto(
    Guid Id,
    NotificationType Type,
    string Title,
    string Body,
    bool IsRead,
    Guid? RelatedId,
    DateTime CreatedAt);
