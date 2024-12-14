using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Extensions;
using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.UserRoleManagement.Commands.Add;

public class CommandHandler: CommandHandlerBase<Command>
{
    
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHashService _hashService;
    
    public CommandHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IHashService hashService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _hashService = hashService;
    }
    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var applicationRole = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (applicationRole is null || string.IsNullOrEmpty(applicationRole.Name))
        {
            return Result.WithFailure(Error.WithMessage("Rol bulunamadı."));
        }

        var userId = _hashService.Decode(request.UserId);

        var applicationUser = await _userManager.FindByIdAsync(userId.ToString());

        if (applicationUser is null)
        {
            return Result.WithFailure(Error.WithMessage("Kullanıcı bulunamadı."));
        }
        
        var roles = await _userManager.GetRolesAsync(applicationUser);

        if (roles.Contains(applicationRole.Name))
        {
            return Result.WithFailure(Error.WithMessage("Kişinin bu rolü zaten mevcut."));
        }

        var result = await _userManager.AddToRoleAsync(applicationUser, applicationRole.Name);
        
        if (!result.Succeeded)
        {
            return Result.WithFailure(Error.WithMessage(result.GetErrorMessage("Rol kaydedilemedi")));
        }

        return Result.WithSuccess();
    }
}