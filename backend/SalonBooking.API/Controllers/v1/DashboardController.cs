using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Appointments;
using SalonBooking.Application.Features.Appointments.Queries.GetCalendarAppointments;
using SalonBooking.Application.Features.Dashboard;
using SalonBooking.Application.Features.Dashboard.Queries.GetEstablishmentDashboard;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1/establishments/me")]
[Authorize(Roles = "Establishment")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator) => _mediator = mediator;

    /// <summary>Retorna métricas do dashboard do estabelecimento</summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEstablishmentDashboardQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna agendamentos formatados para o calendário (FullCalendar)</summary>
    [HttpGet("calendar")]
    [ProducesResponseType(typeof(IEnumerable<CalendarEventDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCalendar(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] Guid? professionalId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCalendarAppointmentsQuery(from, to, professionalId), cancellationToken);
        return Ok(result);
    }
}
