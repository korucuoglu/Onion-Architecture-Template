﻿namespace MyTemplate.Application.UserManagement.Update;
public class Validator : ValidatorBase<Command>
{
    public Validator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
