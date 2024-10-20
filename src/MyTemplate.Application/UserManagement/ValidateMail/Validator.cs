namespace MyTemplate.Application.UserManagement.ValidateMail;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}