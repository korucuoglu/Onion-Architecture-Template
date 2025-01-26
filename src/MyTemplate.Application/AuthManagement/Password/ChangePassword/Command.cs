namespace MyTemplate.Application.AuthManagement.Password.ChangePassword;

public class Command : CommandBase
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}