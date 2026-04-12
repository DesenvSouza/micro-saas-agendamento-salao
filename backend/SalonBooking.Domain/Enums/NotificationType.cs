namespace SalonBooking.Domain.Enums;

public enum NotificationType
{
    AppointmentCreated = 0,
    AppointmentConfirmed = 1,
    AppointmentCancelled = 2,
    AppointmentCompleted = 3,
    AppointmentReminder = 4,
    AppointmentNoShow = 5,
    General = 99
}
