namespace MyTemplate.Application.AuthManagement.Register;

public class UserCreatedEvent : NotificationBase
{
    public ApplicationUser User { get; set; }
}