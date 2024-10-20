namespace MyTemplate.Application.UserManagement.Register;

public class UserCreatedEvent : NotificationBase
{
    public ApplicationUser User { get; set; }
}