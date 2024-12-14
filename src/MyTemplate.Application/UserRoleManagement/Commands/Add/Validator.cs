namespace MyTemplate.Application.UserRoleManagement.Commands.Add;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}