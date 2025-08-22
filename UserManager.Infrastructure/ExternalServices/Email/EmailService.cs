using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using UserManager.Application.Configuration;
using UserManager.Domain.Entities;
using UserManager.Domain.Interfaces;

namespace UserManager.Infrastructure.ExternalServices.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(UserRecord user)
        {
            _logger.LogInformation("Sending email with user details from {EmailFrom} to {EmailTo}", _emailSettings.SenderEmail, user.Email);
            using var message = new MimeMessage();
            GenerateMessage(message, user);

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.LogInformation("Email sent successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email was not sent successfully");
                return false;
            }
        }

        private void GenerateMessage(MimeMessage message, UserRecord user)
        {
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", $"{user.Email}"));
            message.Subject = "Novi korisnički zapis";

            message.Body = new TextPart("plain")
            {
                Text = $"""
                    Poštovani {user.FirstName} {user.LastName},

                    Sljedeći korisnički zapis je upisan u bazu:
                        Ime: {user.FirstName}
                        Prezime: {user.LastName}
                        Email: {user.Email}
                        Korisničko ime: {user.Username ?? "/"}
                        Telefon: {user.Phone ?? "/"}
                        Web stranica: {user.Website ?? "/"}
                        Adresa 
                            Grad: {user.Address?.City ?? "/"}
                            Poštanski broj: {user.Address?.ZipCode ?? "/"}
                            Ulica: {user.Address?.Street ?? "/"}
                            Stan: {user.Address?.Suite ?? "/"}
                            Geografska širina: {user.Address?.Lat ?? "/"}
                            Geografska dužina: {user.Address?.Lng ?? "/"}
                        Tvrtka
                            Naziv: {user.Company?.Name ?? "/"}
                            Slogan: {user.Company?.CatchPhrase ?? "/"}
                            Poslovni model: {user.Company?.Bs ?? "/"}


                    Lijep pozdrav,
                    {_emailSettings.SenderName}
                    """
            };
        }
    }
}
