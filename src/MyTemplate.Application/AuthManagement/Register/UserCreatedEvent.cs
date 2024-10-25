using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.AuthManagement.Register;

public class UserCreatedEvent : NotificationBase
{
    public required ApplicationUser User { get; set; }
}