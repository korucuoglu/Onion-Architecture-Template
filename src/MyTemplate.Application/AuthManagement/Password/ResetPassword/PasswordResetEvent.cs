namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

public class PasswordResetEvent: NotificationBase
{
    public ApplicationUser User { get; set; }
}