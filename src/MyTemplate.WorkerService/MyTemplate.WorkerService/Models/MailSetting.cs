namespace MyTemplate.WorkerService.Models;

public class MailSetting
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Address { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}