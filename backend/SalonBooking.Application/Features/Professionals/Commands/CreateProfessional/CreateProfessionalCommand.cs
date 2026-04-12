using MediatR;

namespace SalonBooking.Application.Features.Professionals.Commands.CreateProfessional;

public record CreateProfessionalCommand(
    string Name,
    string? Bio = null,
    IEnumerable<Guid>? ServiceIds = null) : IRequest<ProfessionalDto>;
