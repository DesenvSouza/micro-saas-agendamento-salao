using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAppointmentByIdQueryHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AppointmentDto> Handle(
        GetAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetWithDetailsAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.Appointment), request.Id);

        var userId = _currentUserService.UserId;
        var role = _currentUserService.Role;

        // Clients can only see their own appointments; establishments can see theirs
        if (role == UserRole.Client && appointment.ClientId != userId)
            throw new UnauthorizedAccessException();

        if (role == UserRole.Establishment && appointment.EstablishmentId != userId)
            throw new UnauthorizedAccessException();

        return appointment.ToDto();
    }
}
