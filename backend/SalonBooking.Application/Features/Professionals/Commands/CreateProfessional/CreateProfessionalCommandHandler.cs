using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Commands.CreateProfessional;

public class CreateProfessionalCommandHandler : IRequestHandler<CreateProfessionalCommand, ProfessionalDto>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateProfessionalCommandHandler(
        IProfessionalRepository professionalRepository,
        IServiceRepository serviceRepository,
        ICurrentUserService currentUserService)
    {
        _professionalRepository = professionalRepository;
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ProfessionalDto> Handle(
        CreateProfessionalCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var professional = Professional.Create(establishmentId, request.Name, request.Bio);

        // Associate services (validate ownership)
        if (request.ServiceIds is not null)
        {
            foreach (var serviceId in request.ServiceIds.Distinct())
            {
                var service = await _serviceRepository.GetByIdAsync(serviceId, cancellationToken)
                    ?? throw new EntityNotFoundException(nameof(Service), serviceId);

                if (service.EstablishmentId != establishmentId)
                    throw new BusinessRuleViolationException($"Serviço '{serviceId}' não pertence ao seu estabelecimento.");

                professional.ProfessionalServices.Add(ProfessionalService.Create(professional.Id, serviceId));
            }
        }

        await _professionalRepository.AddAsync(professional, cancellationToken);

        return professional.ToDto();
    }
}
