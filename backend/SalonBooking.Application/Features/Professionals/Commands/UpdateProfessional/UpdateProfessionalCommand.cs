using MediatR;

namespace SalonBooking.Application.Features.Professionals.Commands.UpdateProfessional;

public record UpdateProfessionalCommand(
    Guid Id,
    string Name,
    string? Bio = null,
    string? PhotoUrl = null,
    IEnumerable<Guid>? ServiceIds = null) : IRequest<ProfessionalDto>;
