namespace SalonBooking.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
    Task SendAppointmentConfirmationAsync(string to, string clientName, string establishmentName,
        DateTime startTime, string serviceName, CancellationToken cancellationToken = default);
    Task SendAppointmentCancellationAsync(string to, string recipientName,
        DateTime startTime, string establishmentName, CancellationToken cancellationToken = default);
}
