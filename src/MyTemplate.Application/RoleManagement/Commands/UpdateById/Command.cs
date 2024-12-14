namespace MyTemplate.Application.RoleManagement.Commands.UpdateById;

public class Command: CommandBase
{
    public int Id { get; set; }
    public string Name { get; set; }
}