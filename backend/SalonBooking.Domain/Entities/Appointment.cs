using SalonBooking.Domain.Entities.Base;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Events;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.ValueObjects;

namespace SalonBooking.Domain.Entities;

public class Appointment : AuditableEntity
{
    public Guid ClientId { get; private set; }
    public Guid EstablishmentId { get; private set; }
    public Guid ProfessionalId { get; private set; }
    public Guid ServiceId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public string? ClientNotes { get; private set; }
    public string? EstablishmentNotes { get; private set; }

    public Client Client { get; private set; } = default!;
    public Establishment Establishment { get; private set; } = default!;
    public Professional Professional { get; private set; } = default!;
    public Service Service { get; private set; } = default!;

    private Appointment() { }

    public static Appointment Create(
        Guid clientId,
        Guid establishmentId,
        Guid professionalId,
        Guid serviceId,
        DateTime startTime,
        int durationMinutes,
        decimal price,
        string currency,
        bool autoConfirm,
        string? clientNotes = null)
    {
        var appointment = new Appointment
        {
            ClientId = clientId,
            EstablishmentId = establishmentId,
            ProfessionalId = professionalId,
            ServiceId = serviceId,
            StartTime = startTime,
            EndTime = startTime.AddMinutes(durationMinutes),
            Price = price,
            Currency = currency,
            Status = autoConfirm ? AppointmentStatus.Confirmed : AppointmentStatus.Pending,
            ClientNotes = clientNotes?.Trim()
        };

        appointment.RaiseDomainEvent(new AppointmentCreatedEvent(
            appointment.Id, clientId, establishmentId, professionalId, startTime));

        return appointment;
    }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Pending)
            throw new BusinessRuleViolationException("Only pending appointments can be confirmed.");

        Status = AppointmentStatus.Confirmed;
        RaiseDomainEvent(new AppointmentConfirmedEvent(Id, ClientId, EstablishmentId));
        SetUpdatedAt();
    }

    public void Cancel(bool cancelledByClient)
    {
        if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
            throw new BusinessRuleViolationException("Cannot cancel a completed or already cancelled appointment.");

        if (cancelledByClient && StartTime <= DateTime.UtcNow.AddHours(2))
            throw new BusinessRuleViolationException("Cannot cancel within 2 hours of appointment start time.");

        Status = AppointmentStatus.Cancelled;
        RaiseDomainEvent(new AppointmentCancelledEvent(Id, ClientId, EstablishmentId, cancelledByClient));
        SetUpdatedAt();
    }

    public void Complete()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new BusinessRuleViolationException("Only confirmed appointments can be completed.");

        Status = AppointmentStatus.Completed;
        RaiseDomainEvent(new AppointmentCompletedEvent(Id, ClientId, EstablishmentId, new Money(Price, Currency)));
        SetUpdatedAt();
    }

    public void MarkNoShow()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new BusinessRuleViolationException("Only confirmed appointments can be marked as no-show.");

        Status = AppointmentStatus.NoShow;
        SetUpdatedAt();
    }

    public void AddEstablishmentNote(string note)
    {
        EstablishmentNotes = note.Trim();
        SetUpdatedAt();
    }

    public bool OverlapsWith(DateTime start, DateTime end) =>
        start < EndTime && end > StartTime;

    public Money GetMoney() => new(Price, Currency);
}
