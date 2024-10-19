namespace MyTemplate.Application.UserManagement.ChangePassword;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.NewPassword).EmailAddress();
        RuleFor(x => x.CurrentPassword).NotEmpty();
    }
}