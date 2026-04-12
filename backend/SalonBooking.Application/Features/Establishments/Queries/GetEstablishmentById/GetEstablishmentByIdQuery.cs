using MediatR;

namespace SalonBooking.Application.Features.Establishments.Queries.GetEstablishmentById;

public record GetEstablishmentByIdQuery(Guid Id) : IRequest<EstablishmentDto>;
