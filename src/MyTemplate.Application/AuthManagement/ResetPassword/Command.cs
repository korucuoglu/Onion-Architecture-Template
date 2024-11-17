namespace MyTemplate.Application.AuthManagement.ResetPassword;

public class Command: CommandBase
{
    public required string Email { get; set; }
}