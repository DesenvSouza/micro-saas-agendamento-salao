using MediatR;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalById;

public record GetProfessionalByIdQuery(Guid Id) : IRequest<ProfessionalDto>;
