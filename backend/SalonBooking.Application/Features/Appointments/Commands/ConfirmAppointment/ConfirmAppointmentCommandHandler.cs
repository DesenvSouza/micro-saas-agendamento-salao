using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Commands.ConfirmAppointment;

public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public ConfirmAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AppointmentDto> Handle(
        ConfirmAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var appointment = await _appointmentRepository.GetWithDetailsAsync(
            request.AppointmentId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Appointment), request.AppointmentId);

        if (appointment.EstablishmentId != establishmentId)
            throw new UnauthorizedAccessException();

        appointment.Confirm();
        _appointmentRepository.Update(appointment);

        var notification = Notification.Create(
            appointment.ClientId,
            NotificationType.AppointmentConfirmed,
            "Agendamento confirmado",
            $"Seu agendamento para {appointment.Service?.Name} em {appointment.StartTime:dd/MM/yyyy HH:mm} foi confirmado.",
            appointment.Id);

        await _notificationRepository.AddAsync(notification, cancellationToken);

        return appointment.ToDto();
    }
}
