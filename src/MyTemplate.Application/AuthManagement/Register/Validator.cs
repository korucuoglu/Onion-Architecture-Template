using Common.Constants;

namespace MyTemplate.Application.AuthManagement.Register;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.RePassword).NotEmpty();
        RuleFor(x => new { x.Password, x.RePassword }).Must(x => x.Password == x.RePassword).WithMessage(CustomResponseMessages.RePasswordError);
    }
}