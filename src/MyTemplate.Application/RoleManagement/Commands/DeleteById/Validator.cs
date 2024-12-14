namespace MyTemplate.Application.RoleManagement.Commands.DeleteById;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}