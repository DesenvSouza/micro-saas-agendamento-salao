using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalonBooking.Application.Common.Interfaces;

namespace SalonBooking.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        // TODO Sprint 4: implementar envio real via MailKit/SMTP
        _logger.LogInformation("Email queued to: {To} | Subject: {Subject}", to, subject);
        await Task.CompletedTask;
    }

    public async Task SendAppointmentConfirmationAsync(
        string to,
        string clientName,
        string establishmentName,
        DateTime startTime,
        string serviceName,
        CancellationToken cancellationToken = default)
    {
        var subject = $"Agendamento confirmado - {establishmentName}";
        var body = $"""
            <h2>Seu agendamento foi confirmado!</h2>
            <p>Olá, <strong>{clientName}</strong>!</p>
            <p>Seu agendamento foi confirmado com os seguintes detalhes:</p>
            <ul>
                <li><strong>Estabelecimento:</strong> {establishmentName}</li>
                <li><strong>Serviço:</strong> {serviceName}</li>
                <li><strong>Data e Hora:</strong> {startTime:dd/MM/yyyy HH:mm}</li>
            </ul>
            <p>Até logo!</p>
            """;

        await SendAsync(to, subject, body, cancellationToken);
    }

    public async Task SendAppointmentCancellationAsync(
        string to,
        string recipientName,
        DateTime startTime,
        string establishmentName,
        CancellationToken cancellationToken = default)
    {
        var subject = $"Agendamento cancelado - {establishmentName}";
        var body = $"""
            <h2>Agendamento cancelado</h2>
            <p>Olá, <strong>{recipientName}</strong>!</p>
            <p>O agendamento do dia <strong>{startTime:dd/MM/yyyy HH:mm}</strong> em <strong>{establishmentName}</strong> foi cancelado.</p>
            <p>Em caso de dúvidas, entre em contato com o estabelecimento.</p>
            """;

        await SendAsync(to, subject, body, cancellationToken);
    }
}
