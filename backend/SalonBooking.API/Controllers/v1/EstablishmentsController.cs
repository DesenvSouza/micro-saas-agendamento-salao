using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonBooking.Application.Features.Establishments;
using SalonBooking.Application.Features.Establishments.Commands.UpdateEstablishmentProfile;
using SalonBooking.Application.Features.Establishments.Queries.GetEstablishmentById;

namespace SalonBooking.API.Controllers.v1;

[ApiController]
[Route("api/v1/establishments")]
public class EstablishmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EstablishmentsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Retorna o perfil público de um estabelecimento</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EstablishmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEstablishmentByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>Atualiza o perfil do estabelecimento autenticado</summary>
    [HttpPut("me")]
    [Authorize(Roles = "Establishment")]
    [ProducesResponseType(typeof(EstablishmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateEstablishmentProfileCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
