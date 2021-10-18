using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using Travel.Application.Dtos.Email;
using Travel.Application.Common.Exceptions;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Travel.Domain.Settings;

namespace Travel.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly MailSettings _mailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<MailSettings> mailSettingsoptions)
        {
            _logger = logger;
            _mailSettings = mailSettingsoptions.Value;
        }

        public async Task SendAsync(EmailDto emailDto)
        {
            try
            {
                var email = new MimeMessage()
                {
                    Sender = MailboxAddress.Parse(emailDto.From ?? _mailSettings.EmailFrom),
                };

                email.To.Add(MailboxAddress.Parse(emailDto.To));
                email.Subject = emailDto.Subject;

                var msgBodyBuilder = new BodyBuilder()
                {
                    HtmlBody = emailDto.Body
                };

                email.Body = msgBodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                throw new ApiException(exception.Message);
            }
        }
    }
}
