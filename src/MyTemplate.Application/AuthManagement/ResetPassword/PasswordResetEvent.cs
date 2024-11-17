using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.AuthManagement.ResetPassword;

public class PasswordResetEvent: NotificationBase
{
    public ApplicationUser User { get; set; }
}