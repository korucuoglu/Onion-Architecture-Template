namespace MyTemplate.Application.AuthManagement.Password.ValidateResetPasswordToken;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}