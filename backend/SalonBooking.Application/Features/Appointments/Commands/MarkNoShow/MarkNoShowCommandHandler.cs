using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Commands.MarkNoShow;

public class MarkNoShowCommandHandler : IRequestHandler<MarkNoShowCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkNoShowCommandHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AppointmentDto> Handle(
        MarkNoShowCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var appointment = await _appointmentRepository.GetWithDetailsAsync(
            request.AppointmentId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Appointment), request.AppointmentId);

        if (appointment.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException();

        appointment.MarkNoShow();
        _appointmentRepository.Update(appointment);

        return appointment.ToDto();
    }
}
