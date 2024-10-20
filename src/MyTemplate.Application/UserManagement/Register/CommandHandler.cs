﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Interfaces;

namespace MyTemplate.Application.UserManagement.Register;

public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly ISettingService _settingService;
    private readonly IMediator _mediator;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService, IMediator mediator)
    {
        _userManager = userManager;
        _settingService = settingService;
        _mediator = mediator;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser()
        {
            UserName = request.Username,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.Select(x => x.Description).FirstOrDefault() ?? "Kullanıcı kaydı yapılamadı.";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        var emailConfirmRequired = _settingService.EmailConfirmRequired();

        if (emailConfirmRequired)
        {
            await _mediator.Publish<UserCreatedEvent>(new()
            {
                User = user,
            }, cancellationToken);
        }

        return Result.WithSuccess();
    }
}