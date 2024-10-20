namespace MyTemplate.Application.AuthManagement.ChangePassword;

public class Command : CommandBase
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}