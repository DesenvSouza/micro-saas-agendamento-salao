using MediatR;

namespace SalonBooking.Application.Features.Notifications.Queries.GetUserNotifications;

public record GetUserNotificationsQuery(
    int Skip = 0,
    int Take = 20) : IRequest<GetUserNotificationsResponse>;

public record GetUserNotificationsResponse(
    IEnumerable<NotificationDto> Items,
    int UnreadCount);
