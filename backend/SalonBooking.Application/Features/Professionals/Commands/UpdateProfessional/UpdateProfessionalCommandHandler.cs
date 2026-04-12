using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.UpdateProfessional;

public class UpdateProfessionalCommandHandler : IRequestHandler<UpdateProfessionalCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProfessionalCommandHandler(
        IProfessionalRepository professionalRepository,
        IServiceRepository serviceRepository,
        ICurrentUserService currentUserService)
    {
        _professionalRepository = professionalRepository;
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ProfessionalDto> Handle(
        UpdateProfessionalCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = await _professionalRepository.GetWithScheduleAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Professional), request.Id);

        if (professional.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException("Profissional não pertence ao seu estabelecimento.");

        professional.Update(request.Name, request.Bio, request.PhotoUrl);

        // Replace services if provided
        if (request.ServiceIds is not null)
        {
            professional.ProfessionalServices.Clear();

            foreach (var serviceId in request.ServiceIds.Distinct())
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(Service), serviceId);

                if (service.EstablishmentId != establishmentId)
                    throw new BusinessRuleViolationException($"Serviço '{serviceId}' não pertence ao seu estabelecimento.");

                professional.ProfessionalServices.Add(ProfessionalService.Create(professional.Id, serviceId));
            }
        }

        _professionalRepository.Update(professional);

        return professional.ToDto();
    }
}
