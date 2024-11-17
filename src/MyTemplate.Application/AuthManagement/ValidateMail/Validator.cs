namespace MyTemplate.Application.AuthManagement.ValidateMail;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}