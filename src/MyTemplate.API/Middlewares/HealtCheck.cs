// ReSharper disable All
namespace MyTemplate.API.Middlewares;

public static class HealtCheck
{
    public static void UseHealtCheck(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/healt", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
        {
            ResponseWriter = async (context, _) =>
            {
                await context.Response.WriteAsync("OK");
            }
        });
    }
}