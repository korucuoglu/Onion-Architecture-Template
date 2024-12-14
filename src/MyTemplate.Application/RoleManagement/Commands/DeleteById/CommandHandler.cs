using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Extensions;
using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.RoleManagement.Commands.DeleteById;

public class CommandHandler: CommandHandlerBase<Command>
{
    
    private readonly RoleManager<ApplicationRole> _roleManager;
    
    public CommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var applicationRole = await _roleManager.FindByIdAsync(request.Id.ToString());
        
        if (applicationRole is null)
        {
            return Result.WithFailure(Error.WithMessage("Rol bulunamadı"));
        }

        var result = await _roleManager.DeleteAsync(applicationRole);

        if (!result.Succeeded)
        {
            return Result.WithFailure(Error.WithMessage(result.GetErrorMessage("Rol silinemedi")));
        }

        return Result.WithSuccess();
    }
}