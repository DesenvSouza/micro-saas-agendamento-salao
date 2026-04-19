using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
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
        var host = _configuration["Email:SmtpHost"];

        // If SMTP is not configured, log and skip (dev mode)
        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogInformation("Email (not sent — SMTP not configured) | To: {To} | Subject: {Subject}", to, subject);
            return;
        }

        var port = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var username = _configuration["Email:Username"] ?? string.Empty;
        var password = _configuration["Email:Password"] ?? string.Empty;
        var fromAddress = _configuration["Email:FromAddress"] ?? username;
        var fromName = _configuration["Email:FromName"] ?? "Salon Booking";

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);

            if (!string.IsNullOrWhiteSpace(username))
                await client.AuthenticateAsync(username, password, cancellationToken);

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Email sent | To: {To} | Subject: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email | To: {To} | Subject: {Subject}", to, subject);
        }
    }

    public async Task SendAppointmentConfirmationAsync(
        string to,
        string clientName,
        string establishmentName,
        DateTime startTime,
        string serviceName,
        CancellationToken cancellationToken = default)
    {
        var subject = $"Agendamento confirmado — {establishmentName}";
        var body = $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
              <h2 style="color: #28a745;">✅ Agendamento confirmado!</h2>
              <p>Olá, <strong>{clientName}</strong>!</p>
              <p>Seu agendamento foi confirmado com os seguintes detalhes:</p>
              <table style="border-collapse: collapse; width: 100%;">
                <tr><td style="padding: 8px; font-weight: bold;">Estabelecimento</td><td style="padding: 8px;">{establishmentName}</td></tr>
                <tr style="background: #f8f9fa;"><td style="padding: 8px; font-weight: bold;">Serviço</td><td style="padding: 8px;">{serviceName}</td></tr>
                <tr><td style="padding: 8px; font-weight: bold;">Data e Hora</td><td style="padding: 8px;">{startTime:dd/MM/yyyy 'às' HH:mm}</td></tr>
              </table>
              <p style="margin-top: 16px; color: #6c757d;">Até breve! 💈</p>
            </div>
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
        var subject = $"Agendamento cancelado — {establishmentName}";
        var body = $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
              <h2 style="color: #dc3545;">❌ Agendamento cancelado</h2>
              <p>Olá, <strong>{recipientName}</strong>!</p>
              <p>O agendamento abaixo foi <strong>cancelado</strong>:</p>
              <table style="border-collapse: collapse; width: 100%;">
                <tr><td style="padding: 8px; font-weight: bold;">Estabelecimento</td><td style="padding: 8px;">{establishmentName}</td></tr>
                <tr style="background: #f8f9fa;"><td style="padding: 8px; font-weight: bold;">Data e Hora</td><td style="padding: 8px;">{startTime:dd/MM/yyyy 'às' HH:mm}</td></tr>
              </table>
              <p style="margin-top: 16px; color: #6c757d;">Em caso de dúvidas, entre em contato com o estabelecimento.</p>
            </div>
            """;

        await SendAsync(to, subject, body, cancellationToken);
    }
}
