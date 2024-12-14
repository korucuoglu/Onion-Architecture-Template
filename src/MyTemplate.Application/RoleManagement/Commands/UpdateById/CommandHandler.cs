using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Extensions;
using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.RoleManagement.Commands.UpdateById;

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

        if (applicationRole.Name!.Equals(request.Name, StringComparison.OrdinalIgnoreCase))
        {
            return Result.WithFailure(Error.WithMessage("Rol adında bir değişiklik bulunamadı"));
        }

        applicationRole.Name = request.Name;

        var result = await _roleManager.UpdateAsync(applicationRole);

        if (!result.Succeeded)
        {
            return Result.WithFailure(Error.WithMessage(result.GetErrorMessage("Rol güncellenemedi")));
        }

        return Result.WithSuccess();
    }
}