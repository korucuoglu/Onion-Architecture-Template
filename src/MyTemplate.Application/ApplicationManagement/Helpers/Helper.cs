using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyTemplate.Application.ApplicationManagement.Helpers;

public static class Helper
{
    public static string GetUserId(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.User?.FindFirstValue("id")
                 ?? throw new Exception("UserId değeri alınamadı");
    }
}