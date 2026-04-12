using MediatR;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalById;

public class GetProfessionalByIdQueryHandler : IRequestHandler<GetProfessionalByIdQuery, ProfessionalDto>
{
    private readonly IProfessionalRepository _professionalRepository;

    public GetProfessionalByIdQueryHandler(IProfessionalRepository professionalRepository) =>
        _professionalRepository = professionalRepository;

    public async Task<ProfessionalDto> Handle(
        GetProfessionalByIdQuery request,
        CancellationToken cancellationToken)
    {
        var professional = await _professionalRepository.GetWithScheduleAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Professional), request.Id);

        return professional.ToDto();
    }
}
