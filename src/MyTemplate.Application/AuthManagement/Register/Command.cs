namespace MyTemplate.Application.AuthManagement.Register;

public class Command : CommandBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
}