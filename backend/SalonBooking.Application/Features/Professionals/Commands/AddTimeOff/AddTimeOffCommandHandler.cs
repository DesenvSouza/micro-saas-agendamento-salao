using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.AddTimeOff;

public class AddTimeOffCommandHandler : IRequestHandler<AddTimeOffCommand, TimeOffDto>
{
    private readonly ITimeOffRepository _timeOffRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddTimeOffCommandHandler(
        ITimeOffRepository timeOffRepository,
        IProfessionalRepository professionalRepository,
        ICurrentUserService currentUserService)
    {
        _timeOffRepository = timeOffRepository;
        _professionalRepository = professionalRepository;
        _currentUserService = currentUserService;
    }

    public async Task<TimeOffDto> Handle(AddTimeOffCommand request, CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Professional), request.ProfessionalId);

        if (professional.EstablishmentId != establishmentId)
            throw new BusinessRuleViolationException("Profissional não pertence ao seu estabelecimento.");

        // Check for overlapping time offs
        var overlapping = await _timeOffRepository.GetOverlappingAsync(
            request.ProfessionalId,
            request.StartDateTime,
            request.EndDateTime,
            cancellationToken);

        if (overlapping.Any())
            throw new BusinessRuleViolationException("Já existe uma folga cadastrada neste período.");

        var timeOff = TimeOff.Create(
            request.ProfessionalId,
            request.StartDateTime,
            request.EndDateTime,
            request.Reason);

        await _timeOffRepository.AddAsync(timeOff, cancellationToken);

        return timeOff.ToDto();
    }
}
