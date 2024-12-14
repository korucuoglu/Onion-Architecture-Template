namespace MyTemplate.Application.RoleManagement.Commands.UpdateById;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}