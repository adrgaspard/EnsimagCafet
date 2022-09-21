using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EnsimagCafet.MailKit
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public MailKitEmailSender(IOptions<MailKitEmailSenderOptions> optionsAccessor, ILogger<MailKitEmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public MailKitEmailSenderOptions Options { get; }

        public async Task SendEmailAsync(string destinationEmail, string subject, string content)
        {
            using SmtpClient smtpClient = new();
            string destinationName = "[NULL]";
            try
            {
                destinationName = string.Join(' ', destinationEmail.Split('@').First().ToLower()
                .Split('.').Select(part => part.Length > 1 ? char.ToUpper(part[0]) + part[1..] : part));
                MimeMessage message = new();
                message.From.Add(new MailboxAddress(Options.SenderName, Options.SenderEmail));
                message.To.Add(new MailboxAddress(destinationName, destinationEmail));
                message.Subject = subject;
                message.Body = new TextPart((TextFormat)Options.ContentType) { Text = content };
                smtpClient.CheckCertificateRevocation = Options.CheckCertificateRevocation;
                smtpClient.Connect(Options.SmtpHost, Options.SmtpPort, Options.UseSsl);
                _ = smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                smtpClient.Authenticate(Options.SenderEmail, Options.SenderPassword);
                string response = smtpClient.Send(message);
                _logger.LogInformation($"Email to {destinationEmail} ({destinationName}), response: {response}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Email failure to {destinationEmail} ({destinationName}), error: {exception.Message}");
            }
            await smtpClient.DisconnectAsync(true);
        }
    }
}