using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.DeleteTimeOff;

public class DeleteTimeOffCommandHandler : IRequestHandler<DeleteTimeOffCommand>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTimeOffCommandHandler(
        ITimeOffRepository timeOffRepository,
        IProfessionalRepository professionalRepository,
        ICurrentUserService currentUserService)
    {
        _timeOffRepository = timeOffRepository;
        _professionalRepository = professionalRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteTimeOffCommand request, CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Professional), request.ProfessionalId);

        if (professional.EstablishmentId != establishmentId)
            throw new BusinessRuleViolationException("Profissional não pertence ao seu estabelecimento.");

        var timeOff = await _timeOffRepository.GetByIdAsync(request.TimeOffId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(TimeOff), request.TimeOffId);

        if (timeOff.ProfessionalId != request.ProfessionalId)
            throw new BusinessRuleViolationException("Folga não pertence ao profissional informado.");

        _timeOffRepository.Remove(timeOff);
    }
}
