namespace MyTemplate.Application.UserManagement.Login;

public class Command : CommandBase<Dto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}