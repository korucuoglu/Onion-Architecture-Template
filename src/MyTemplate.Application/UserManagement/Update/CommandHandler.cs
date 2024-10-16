﻿using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.UserManagement.Update;
public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHashService _hashService;
    private readonly IUnitOfWork _unitOfWork;
    public CommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IHashService hashService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _hashService = hashService;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var encodedUserId = Helper.GetUserId(_httpContextAccessor);

        var userId = _hashService.Decode(encodedUserId);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        var isEmailChanged = user.Email!.Equals(request.Email) == false; // email değişti mi?

        user.EmailConfirmed = isEmailChanged ? false : user.EmailConfirmed; // Email'ini değiştirmesi durumunda "false" olur. 
        user.Email = request.Email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? "Hata meydana geldi";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        await SendMailAsync();

        return Result.WithSuccess();
    }

    private async Task SendMailAsync()
    {
        await using var unitOfWork = _unitOfWork.EF;

        var settingRepository = unitOfWork.GetRepository<Setting, int>();

        var setting = await settingRepository.FirstOrDefaultAsync(x => x.Key == "EmailConfirmRequired");

        var settingValue = Helper.GetSettingValue(setting);

        if (settingValue is bool emailConfirmRequired && emailConfirmRequired)
        {
            // await SendMail();
        }
    }
}
