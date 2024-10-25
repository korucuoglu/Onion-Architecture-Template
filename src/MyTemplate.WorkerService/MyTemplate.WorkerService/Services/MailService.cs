using System.Net;
using System.Net.Mail;
using Common.Events;
using Microsoft.Extensions.Options;
using MyTemplate.WorkerService.Models;

namespace MyTemplate.WorkerService.Services;

public class MailService
{
    private readonly MailSetting _mailSetting;

    public MailService(IOptions<MailSetting> mailSettings)
    {
        _mailSetting = mailSettings.Value;
    }

    public async Task SendAsync(MailSendEvent mailSendEvent)
    {
        using SmtpClient client = new(_mailSetting.Host)
        {
            Host = _mailSetting.Host,
            Port = _mailSetting.Port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_mailSetting.Username, _mailSetting.Password)
        };

        foreach (var to in mailSendEvent.To)
        {
            using MailMessage mailMessage = new()
            {
                From = new MailAddress(_mailSetting.Address),
                Subject = mailSendEvent.Subject,
                Body = mailSendEvent.Body,
                IsBodyHtml = true // HTML olarak gönderilmesini sağlar
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}