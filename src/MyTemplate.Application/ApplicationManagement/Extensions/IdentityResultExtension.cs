using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.ApplicationManagement.Extensions;

public static class IdentityResultExtension
{
    public static string GetErrorMessage(this IdentityResult identityResult, string? defaultErrorMessage = null)
    {
        return identityResult.Errors.FirstOrDefault()?.Description ?? defaultErrorMessage ?? "";
    }
}