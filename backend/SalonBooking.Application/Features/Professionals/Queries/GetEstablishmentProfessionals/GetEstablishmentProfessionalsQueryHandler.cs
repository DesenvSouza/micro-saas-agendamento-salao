using MediatR;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Queries.GetEstablishmentProfessionals;

public class GetEstablishmentProfessionalsQueryHandler
    : IRequestHandler<GetEstablishmentProfessionalsQuery, IEnumerable<ProfessionalDto>>
{
    private readonly IProfessionalRepository _professionalRepository;

    public GetEstablishmentProfessionalsQueryHandler(IProfessionalRepository professionalRepository) =>
        _professionalRepository = professionalRepository;

    public async Task<IEnumerable<ProfessionalDto>> Handle(
        GetEstablishmentProfessionalsQuery request,
        CancellationToken cancellationToken)
    {
        var professionals = await _professionalRepository
            .GetByEstablishmentAsync(request.EstablishmentId, cancellationToken);

        return professionals.Select(p => p.ToDto());
    }
}
