namespace MyTemplate.Application.UserManagement.ValidateMail;

public class Command : CommandBase
{
    public string UserId { get; set; }
    public string Token { get; set; }
}