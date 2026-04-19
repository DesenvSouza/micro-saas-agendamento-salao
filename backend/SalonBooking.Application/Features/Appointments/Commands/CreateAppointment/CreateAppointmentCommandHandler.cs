using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, AppointmentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ICurrentUserService _currentUserService;

    public CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IProfessionalRepository professionalRepository,
        IServiceRepository serviceRepository,
        IEstablishmentRepository establishmentRepository,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IEmailService emailService,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _professionalRepository = professionalRepository;
        _serviceRepository = serviceRepository;
        _establishmentRepository = establishmentRepository;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _emailService = emailService;
        _currentUserService = currentUserService;
    }

    public async Task<AppointmentDto> Handle(
        CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        // Load and validate service
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Service), request.ServiceId);

        if (!service.IsActive)
            throw new BusinessRuleViolationException("Serviço não está disponível.");

        // Load and validate professional
        var professional = await _professionalRepository.GetWithScheduleAsync(
            request.ProfessionalId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Professional), request.ProfessionalId);

        if (!professional.IsActive)
            throw new BusinessRuleViolationException("Profissional não está disponível.");

        // Validate that the professional offers this service
        bool canPerform = professional.ProfessionalServices.Any(ps => ps.ServiceId == request.ServiceId);
        if (!canPerform)
            throw new BusinessRuleViolationException("O profissional não realiza este serviço.");

        // Load establishment
        var establishment = await _establishmentRepository.GetByIdAsync(
            professional.EstablishmentId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Establishment), professional.EstablishmentId);

        var endTime = request.StartTime.AddMinutes(service.DurationMinutes);

        // Re-verify availability inside the transaction (TOCTOU prevention)
        bool hasConflict = await _appointmentRepository.HasConflictAsync(
            request.ProfessionalId, request.StartTime, endTime, cancellationToken: cancellationToken);

        if (hasConflict)
            throw new BusinessRuleViolationException("O horário selecionado não está mais disponível.");

        var appointment = Appointment.Create(
            clientId,
            establishment.Id,
            request.ProfessionalId,
            request.ServiceId,
            request.StartTime,
            service.DurationMinutes,
            service.Price,
            service.Currency,
            establishment.AutoConfirmAppointments,
            request.ClientNotes);

        await _appointmentRepository.AddAsync(appointment, cancellationToken);

        // In-app notification for establishment
        var notificationType = appointment.Status == AppointmentStatus.Confirmed
            ? NotificationType.AppointmentConfirmed
            : NotificationType.AppointmentCreated;

        var notification = Notification.Create(
            establishment.Id,
            notificationType,
            "Novo agendamento",
            $"Novo agendamento: {service.Name} com {professional.Name} em {request.StartTime:dd/MM/yyyy HH:mm}.",
            appointment.Id);

        await _notificationRepository.AddAsync(notification, cancellationToken);

        // Email to client if auto-confirmed
        if (appointment.Status == AppointmentStatus.Confirmed)
        {
            var client = await _userRepository.GetByIdAsync(clientId, cancellationToken);
            if (client is not null)
                await _emailService.SendAppointmentConfirmationAsync(
                    client.Email,
                    client is Client c ? c.FullName : client.Email,
                    establishment.TradeName,
                    request.StartTime,
                    service.Name,
                    cancellationToken);
        }

        return appointment.ToDto();
    }
}
