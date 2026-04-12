using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Professionals;
using SalonBooking.Application.Features.Professionals.Commands.AddTimeOff;
using SalonBooking.Application.Features.Professionals.Commands.CreateProfessional;
using SalonBooking.Application.Features.Professionals.Commands.DeleteProfessional;
using SalonBooking.Application.Features.Professionals.Commands.DeleteTimeOff;
using SalonBooking.Application.Features.Professionals.Commands.SetWorkingSchedule;
using SalonBooking.Application.Features.Professionals.Commands.UpdateProfessional;
using SalonBooking.Application.Features.Professionals.Queries.GetEstablishmentProfessionals;
using SalonBooking.Application.Features.Professionals.Queries.GetProfessionalById;
using SalonBooking.Application.Features.Professionals.Queries.GetProfessionalSchedule;
using SalonBooking.Application.Features.Professionals.Queries.GetProfessionalTimeOffs;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1")]
public class ProfessionalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfessionalsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Lista os profissionais de um estabelecimento (público)</summary>
    [HttpGet("establishments/{establishmentId:guid}/professionals")]
    [ProducesResponseType(typeof(IEnumerable<ProfessionalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEstablishment(
        Guid establishmentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetEstablishmentProfessionalsQuery(establishmentId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna um profissional por ID</summary>
    [HttpGet("professionals/{id:guid}")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProfessionalByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cria um novo profissional no estabelecimento autenticado</summary>
    [HttpPost("establishments/me/professionals")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProfessionalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Atualiza um profissional do estabelecimento autenticado</summary>
    [HttpPut("establishments/me/professionals/{id:guid}")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(ProfessionalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProfessionalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    /// <summary>Remove (desativa) um profissional do estabelecimento autenticado</summary>
    [HttpDelete("establishments/me/professionals/{id:guid}")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProfessionalCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Define a grade de horários do profissional</summary>
    [HttpPut("establishments/me/professionals/{professionalId:guid}/schedule")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(IEnumerable<WorkingScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> SetSchedule(
        Guid professionalId,
        [FromBody] SetWorkingScheduleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with { ProfessionalId = professionalId }, cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna a grade de horários de um profissional (público)</summary>
    [HttpGet("professionals/{professionalId:guid}/schedule")]
    [ProducesResponseType(typeof(IEnumerable<WorkingScheduleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedule(
        Guid professionalId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProfessionalScheduleQuery(professionalId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Adiciona uma folga ao profissional</summary>
    [HttpPost("establishments/me/professionals/{professionalId:guid}/timeoffs")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(TimeOffDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AddTimeOff(
        Guid professionalId,
        [FromBody] AddTimeOffCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with { ProfessionalId = professionalId }, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Lista as folgas de um profissional (público)</summary>
    [HttpGet("professionals/{professionalId:guid}/timeoffs")]
    [ProducesResponseType(typeof(IEnumerable<TimeOffDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimeOffs(
        Guid professionalId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProfessionalTimeOffsQuery(professionalId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Remove uma folga do profissional</summary>
    [HttpDelete("establishments/me/professionals/{professionalId:guid}/timeoffs/{timeOffId:guid}")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTimeOff(
        Guid professionalId,
        Guid timeOffId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTimeOffCommand(professionalId, timeOffId), cancellationToken);
        return NoContent();
    }
}
