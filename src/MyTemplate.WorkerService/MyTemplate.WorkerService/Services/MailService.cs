using System.Net;
using System.Net.Mail;
using Common.Models;
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
            Port = _mailSetting.Port,
            Credentials = new NetworkCredential(_mailSetting.Username, _mailSetting.Password),
            EnableSsl = true,
        };

        foreach (var to in mailSendEvent.To)
        {
            await client.SendMailAsync(_mailSetting.Address, to, mailSendEvent.Subject, mailSendEvent.Body);
        }
    }
}