using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Establishments.Commands.UpdateEstablishmentProfile;

public class UpdateEstablishmentProfileCommandHandler
    : IRequestHandler<UpdateEstablishmentProfileCommand, EstablishmentDto>
{
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateEstablishmentProfileCommandHandler(
        IEstablishmentRepository establishmentRepository,
        ICurrentUserService currentUserService)
    {
        _establishmentRepository = establishmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<EstablishmentDto> Handle(
        UpdateEstablishmentProfileCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var establishment = await _establishmentRepository.GetByIdAsync(establishmentId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Establishment), establishmentId);

        establishment.UpdateProfile(
            request.TradeName,
            request.LegalName,
            request.Cnpj,
            request.Phone,
            request.Description,
            request.Street,
            request.Number,
            request.Complement,
            request.Neighborhood,
            request.City,
            request.State,
            request.ZipCode,
            request.Latitude,
            request.Longitude,
            request.AutoConfirmAppointments);

        _establishmentRepository.Update(establishment);

        return establishment.ToDto();
    }
}
