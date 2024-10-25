using Common.Libraries.MediatR.Models;

namespace MyTemplate.API.Middlewares;

public static class GlobalExceptionHandler
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(opt =>
        {
            opt.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(Result.WithFailure(Error.WithMessage("Bilinmeyen bir hata meydana geldi")));
            });
        });
    }
}