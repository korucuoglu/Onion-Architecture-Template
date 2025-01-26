namespace MyTemplate.Application.AuthManagement.Password.ResetPasswordConfirm;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.RePassword).NotEmpty();
        RuleFor(x => new { x.Password, x.RePassword }).Must(x => x.Password == x.RePassword).WithMessage(CustomResponseMessages.RePasswordError);
    }
}