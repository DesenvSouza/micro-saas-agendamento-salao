using MediatR;

namespace SalonBooking.Application.Features.Establishments.Queries.GetNearbyEstablishments;

public record GetNearbyEstablishmentsQuery(
    double Latitude,
    double Longitude,
    double RadiusKm = 10,
    int Skip = 0,
    int Take = 20) : IRequest<IEnumerable<NearbyEstablishmentDto>>;
