namespace MyTemplate.Application.UserManagement.Register;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => new { x.Password, x.RePassword }).Must(x => x.Password == x.RePassword).WithMessage("Parolalar birbirleri ile örtüşmüyor.");
    }
}