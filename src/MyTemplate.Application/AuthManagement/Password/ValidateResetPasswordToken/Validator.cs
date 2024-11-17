namespace MyTemplate.Application.AuthManagement.ValidateResetPasswordToken;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}