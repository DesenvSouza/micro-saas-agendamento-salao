using MediatR;

namespace SalonBooking.Application.Features.Professionals.Queries.GetEstablishmentProfessionals;

public record GetEstablishmentProfessionalsQuery(Guid EstablishmentId) : IRequest<IEnumerable<ProfessionalDto>>;
