using MyTemplate.Application.ApplicationManagement.Common.Constants;

namespace MyTemplate.Application.AuthManagement.ValidateToken;

public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}