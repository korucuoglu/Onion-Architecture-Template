namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

public class Command: CommandBase
{
    public required string Email { get; set; }
}