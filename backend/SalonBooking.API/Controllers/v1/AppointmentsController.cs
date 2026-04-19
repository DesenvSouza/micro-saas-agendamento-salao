using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Appointments;
using SalonBooking.Application.Features.Appointments.Commands.CancelAppointment;
using SalonBooking.Application.Features.Appointments.Commands.CompleteAppointment;
using SalonBooking.Application.Features.Appointments.Commands.ConfirmAppointment;
using SalonBooking.Application.Features.Appointments.Commands.CreateAppointment;
using SalonBooking.Application.Features.Appointments.Commands.MarkNoShow;
using SalonBooking.Application.Features.Appointments.Queries.GetAppointmentById;
using SalonBooking.Application.Features.Appointments.Queries.GetAvailableSlots;
using SalonBooking.Application.Features.Appointments.Queries.GetClientAppointments;
using SalonBooking.Application.Features.Appointments.Queries.GetEstablishmentAppointments;
using SalonBooking.Domain.Enums;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1/appointments")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Retorna os slots disponíveis para agendamento (público)</summary>
    [HttpGet("slots")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<DateTime>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableSlots(
        [FromQuery] Guid professionalId,
        [FromQuery] Guid serviceId,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAvailableSlotsQuery(professionalId, serviceId, date), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna os agendamentos do cliente autenticado</summary>
    [HttpGet("mine")]
    [Authorize(Roles = "Client")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAppointments(
        [FromQuery] bool onlyUpcoming = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetClientAppointmentsQuery(onlyUpcoming), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna os agendamentos do estabelecimento autenticado</summary>
    [HttpGet("establishment")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstablishmentAppointments(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] AppointmentStatus? status,
        [FromQuery] Guid? professionalId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEstablishmentAppointmentsQuery(from, to, status, professionalId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna um agendamento por ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cria um novo agendamento (cliente)</summary>
    [HttpPost]
    [Authorize(Roles = "Client")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAppointmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Confirma um agendamento (estabelecimento)</summary>
    [HttpPost("{id:guid}/confirm")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfirmAppointmentCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cancela um agendamento (cliente ou estabelecimento)</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelAppointmentCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Marca um agendamento como concluído (estabelecimento)</summary>
    [HttpPost("{id:guid}/complete")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteAppointmentCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Registra no-show de um agendamento (estabelecimento)</summary>
    [HttpPost("{id:guid}/no-show")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> NoShow(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new MarkNoShowCommand(id), cancellationToken);
        return Ok(result);
    }
}
