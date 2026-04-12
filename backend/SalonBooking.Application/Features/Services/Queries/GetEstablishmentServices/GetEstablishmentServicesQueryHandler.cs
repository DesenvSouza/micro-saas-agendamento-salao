using MediatR;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Services.Queries.GetEstablishmentServices;

public class GetEstablishmentServicesQueryHandler
    : IRequestHandler<GetEstablishmentServicesQuery, IEnumerable<ServiceDto>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetEstablishmentServicesQueryHandler(IServiceRepository serviceRepository) =>
        _serviceRepository = serviceRepository;

    public async Task<IEnumerable<ServiceDto>> Handle(
        GetEstablishmentServicesQuery request,
        CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetByEstablishmentAsync(
            request.EstablishmentId,
            request.OnlyActive,
            cancellationToken);

        return services.Select(s => s.ToDto());
    }
}
