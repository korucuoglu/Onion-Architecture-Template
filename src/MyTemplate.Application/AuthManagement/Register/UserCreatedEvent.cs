namespace MyTemplate.Application.AuthManagement.Register;

public class UserCreatedEvent : NotificationBase
{
    public required ApplicationUser User { get; set; }
}