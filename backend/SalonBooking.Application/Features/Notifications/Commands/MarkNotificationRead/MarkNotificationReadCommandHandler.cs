using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Notifications.Commands.MarkNotificationRead;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkNotificationReadCommandHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Notification), request.NotificationId);

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException();

        notification.MarkAsRead();
        _notificationRepository.Update(notification);
    }
}
