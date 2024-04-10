using vogue_decor.Application.Common.Options;
using vogue_decor.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace vogue_decor.Persistence.Services
{
    /// <inheritdoc/>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        /// <param name="emailSenderOptions">Конфигурация проекта</param>
        public EmailSender(EmailSenderOptions emailSenderOptions, ILogger logger)
        {
            _options = emailSenderOptions;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_options.Name, _options.Domain));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                _logger.LogInformation("start smtp connection...");
                await client.ConnectAsync(_options.Host, _options.Port, _options.UseSSL);
                _logger.LogInformation("start smtp authentication...");
                await client.AuthenticateAsync(_options.Username, _options.Password);
                _logger.LogInformation("start smtp send...");
                await client.SendAsync(emailMessage);
                _logger.LogInformation("successfully sended!");

                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailAsync(string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Служба безопасности", _options.Domain));
            emailMessage.To.Add(new MailboxAddress("", _options.Username));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {

                await client.ConnectAsync(_options.Host, _options.Port, false);
                await client.AuthenticateAsync(_options.Username, _options.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
