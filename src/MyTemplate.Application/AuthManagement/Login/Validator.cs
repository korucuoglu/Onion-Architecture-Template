namespace MyTemplate.Application.AuthManagement.Login;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}