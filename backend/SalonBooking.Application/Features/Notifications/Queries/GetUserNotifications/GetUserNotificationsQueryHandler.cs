using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler
    : IRequestHandler<GetUserNotificationsQuery, GetUserNotificationsResponse>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUserNotificationsQueryHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetUserNotificationsResponse> Handle(
        GetUserNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var notifications = await _notificationRepository.GetByUserAsync(
            userId, request.Skip, request.Take, cancellationToken);

        var unreadCount = await _notificationRepository.CountUnreadAsync(userId, cancellationToken);

        var items = notifications.Select(n => new NotificationDto(
            n.Id,
            n.Type,
            n.Title,
            n.Body,
            n.IsRead,
            n.RelatedId,
            n.CreatedAt));

        return new GetUserNotificationsResponse(items, unreadCount);
    }
}
