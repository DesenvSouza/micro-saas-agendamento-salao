using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteServiceCommandHandler(
        IServiceRepository serviceRepository,
        ICurrentUserService currentUserService)
    {
        _serviceRepository = serviceRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var service = await _serviceRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Service), request.Id);

        if (service.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException("Serviço não pertence ao seu estabelecimento.");

        // Soft delete — keeps historical appointment data intact
        service.Deactivate();
        _serviceRepository.Update(service);
    }
}
