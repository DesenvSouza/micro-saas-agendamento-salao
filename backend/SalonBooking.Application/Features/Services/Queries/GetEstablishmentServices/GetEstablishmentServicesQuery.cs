using MediatR;

namespace SalonBooking.Application.Features.Services.Queries.GetEstablishmentServices;

public record GetEstablishmentServicesQuery(
    Guid EstablishmentId,
    bool OnlyActive = true) : IRequest<IEnumerable<ServiceDto>>;
