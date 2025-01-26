namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

public class Validator: ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x=> x.Email).NotEmpty().EmailAddress();
    }
}