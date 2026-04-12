using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceDto>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateServiceCommandHandler(
        IServiceRepository serviceRepository,
        ICurrentUserService currentUserService)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ServiceDto> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Service), request.Id);

        if (service.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException("Serviço não pertence ao seu estabelecimento.");

        service.Update(
            request.Name,
            request.Category,
            request.DurationMinutes,
            request.Price,
            request.Description,
            request.SlotIntervalMinutes);

        _serviceRepository.Update(service);

        return service.ToDto();
    }
}
