namespace MyTemplate.Application.UserRoleManagement.Commands.Add;

public class Command: CommandBase
{
    public int RoleId { get; set; }
    public string UserId { get; set; }
}