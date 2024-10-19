namespace MyTemplate.Application.UserManagement.Update;

public class Command : CommandBase
{
    public string Password { get; set; }
    public string Email { get; set; }
}