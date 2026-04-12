using MediatR;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Establishments.Queries.GetEstablishmentById;

public class GetEstablishmentByIdQueryHandler
    : IRequestHandler<GetEstablishmentByIdQuery, EstablishmentDto>
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public GetEstablishmentByIdQueryHandler(IEstablishmentRepository establishmentRepository) =>
        _establishmentRepository = establishmentRepository;

    public async Task<EstablishmentDto> Handle(
        GetEstablishmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var establishment = await _establishmentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Establishment), request.Id);

        return establishment.ToDto();
    }
}
