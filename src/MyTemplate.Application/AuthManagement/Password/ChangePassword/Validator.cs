namespace MyTemplate.Application.AuthManagement.ChangePassword;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.NewPassword).EmailAddress();
        RuleFor(x => x.CurrentPassword).NotEmpty();
    }
}