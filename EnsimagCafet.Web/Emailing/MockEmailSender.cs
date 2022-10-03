using Microsoft.AspNetCore.Identity.UI.Services;

namespace EnsimagCafet.Web.Emailing
{
    public sealed class MockEmailSender : IEmailSender
    {
        private readonly ILogger<MockEmailSender> _logger;

        public MockEmailSender(ILogger<MockEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation($"{typeof(MockEmailSender)}: Email sent to '{email}', subject: '{subject}'.");
            return Task.CompletedTask;
        }
    }
}
