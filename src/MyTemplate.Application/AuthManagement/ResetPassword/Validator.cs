namespace MyTemplate.Application.AuthManagement.ResetPassword;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x=> x.Email).NotEmpty().EmailAddress();
    }
}