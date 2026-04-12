using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Services;
using SalonBooking.Application.Features.Services.Commands.CreateService;
using SalonBooking.Application.Features.Services.Commands.DeleteService;
using SalonBooking.Application.Features.Services.Commands.UpdateService;
using SalonBooking.Application.Features.Services.Queries.GetEstablishmentServices;
using SalonBooking.Application.Features.Services.Queries.GetServiceById;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Lista os serviços de um estabelecimento (público)</summary>
    [HttpGet("establishments/{establishmentId:guid}/services")]
    [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEstablishment(
        Guid establishmentId,
        [FromQuery] bool onlyActive = true,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetEstablishmentServicesQuery(establishmentId, onlyActive), cancellationToken);
        return Ok(result);
    }

    /// <summary>Retorna um serviço por ID</summary>
    [HttpGet("services/{id:guid}")]
    [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetServiceByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Cria um novo serviço no estabelecimento autenticado</summary>
    [HttpPost("establishments/me/services")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Atualiza um serviço do estabelecimento autenticado</summary>
    [HttpPut("establishments/me/services/{id:guid}")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    /// <summary>Remove (desativa) um serviço do estabelecimento autenticado</summary>
    [HttpDelete("establishments/me/services/{id:guid}")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteServiceCommand(id), cancellationToken);
        return NoContent();
    }
}
