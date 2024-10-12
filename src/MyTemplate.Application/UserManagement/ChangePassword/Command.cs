namespace MyTemplate.Application.UserManagement.ChangePassword;
public class Command: CommandBase
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
