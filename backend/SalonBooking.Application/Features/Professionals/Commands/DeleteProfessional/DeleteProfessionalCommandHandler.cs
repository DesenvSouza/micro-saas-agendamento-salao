using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.DeleteProfessional;

public class DeleteProfessionalCommandHandler : IRequestHandler<DeleteProfessionalCommand>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteProfessionalCommandHandler(
        IProfessionalRepository professionalRepository,
        ICurrentUserService currentUserService)
    {
        _professionalRepository = professionalRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteProfessionalCommand request, CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = await _professionalRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Professional), request.Id);

        if (professional.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException("Profissional não pertence ao seu estabelecimento.");

        // Soft delete to preserve appointment history
        professional.Deactivate();
        _professionalRepository.Update(professional);
    }
}
