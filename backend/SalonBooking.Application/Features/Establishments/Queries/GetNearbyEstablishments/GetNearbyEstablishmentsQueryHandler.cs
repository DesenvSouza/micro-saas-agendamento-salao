using MediatR;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Establishments.Queries.GetNearbyEstablishments;

public class GetNearbyEstablishmentsQueryHandler
    : IRequestHandler<GetNearbyEstablishmentsQuery, IEnumerable<NearbyEstablishmentDto>>
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public GetNearbyEstablishmentsQueryHandler(IEstablishmentRepository establishmentRepository) =>
        _establishmentRepository = establishmentRepository;

    public async Task<IEnumerable<NearbyEstablishmentDto>> Handle(
        GetNearbyEstablishmentsQuery request,
        CancellationToken cancellationToken)
    {
        var establishments = await _establishmentRepository.GetNearbyAsync(
            request.Latitude,
            request.Longitude,
            request.RadiusKm,
            request.Skip,
            request.Take,
            cancellationToken);

        return establishments.Select(e => new NearbyEstablishmentDto(
            e.Id,
            e.TradeName,
            e.Description,
            e.Street,
            e.Number,
            e.City,
            e.State,
            e.ZipCode,
            e.Latitude,
            e.Longitude,
            e.LogoUrl,
            HaversineDistance(request.Latitude, request.Longitude, e.Latitude, e.Longitude)));
    }

    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180)
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}
