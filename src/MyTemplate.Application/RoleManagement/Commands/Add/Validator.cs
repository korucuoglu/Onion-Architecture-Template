namespace MyTemplate.Application.RoleManagement.Commands.Add;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}