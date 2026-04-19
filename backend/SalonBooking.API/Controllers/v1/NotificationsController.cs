using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Notifications.Commands.MarkAllNotificationsRead;
using SalonBooking.Application.Features.Notifications.Commands.MarkNotificationRead;
using SalonBooking.Application.Features.Notifications.Queries.GetUserNotifications;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Lista as notificações do usuário autenticado (paginado)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetUserNotificationsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetUserNotificationsQuery(skip, take), cancellationToken);
        return Ok(result);
    }

    /// <summary>Marca uma notificação como lida</summary>
    [HttpPut("{id:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkRead(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkNotificationReadCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Marca todas as notificações como lidas</summary>
    [HttpPut("read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> MarkAllRead(CancellationToken cancellationToken)
    {
        await _mediator.Send(new MarkAllNotificationsReadCommand(), cancellationToken);
        return NoContent();
    }
}
