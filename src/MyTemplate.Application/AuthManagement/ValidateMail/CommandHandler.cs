﻿using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Common.Constants;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.AuthManagement.ValidateMail;

public class CommandHandler : CommandHandlerBase<Command>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISettingService _settingService;
    private readonly ITokenService _tokenService;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService, ITokenService tokenService)
    {
        _userManager = userManager;
        _settingService = settingService;
        _tokenService = tokenService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        if (!_settingService.EmailConfirmRequired())
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        var userId = _tokenService.GetUserId(request.Token, TokenType.MailToken);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        if (user.EmailConfirmed)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? CustomResponseMessages.InvalidUrl;
            
            return Result.WithFailure(Error.WithMessage(errMessage));
        }
        
        return Result.WithSuccess();
    }
}