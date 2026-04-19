using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public CancelAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AppointmentDto> Handle(
        CancelAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();
        var role = _currentUserService.Role;

        var appointment = await _appointmentRepository.GetWithDetailsAsync(
            request.AppointmentId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Appointment), request.AppointmentId);

        bool cancelledByClient = role == UserRole.Client;

        if (cancelledByClient && appointment.ClientId != userId)
            throw new UnauthorizedAccessException();

        if (!cancelledByClient && appointment.EstablishmentId != userId)
            throw new UnauthorizedAccessException();

        appointment.Cancel(cancelledByClient);
        _appointmentRepository.Update(appointment);

        // Notify the other party
        var notifyUserId = cancelledByClient ? appointment.EstablishmentId : appointment.ClientId;
        var actor = cancelledByClient ? "pelo cliente" : "pelo estabelecimento";

        var notification = Notification.Create(
            notifyUserId,
            NotificationType.AppointmentCancelled,
            "Agendamento cancelado",
            $"O agendamento de {appointment.StartTime:dd/MM/yyyy HH:mm} foi cancelado {actor}.",
            appointment.Id);

        await _notificationRepository.AddAsync(notification, cancellationToken);

        return appointment.ToDto();
    }
}
