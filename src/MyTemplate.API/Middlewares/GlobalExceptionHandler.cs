using Common.Exceptions;
using Common.Libraries.MediatR.Models;
using Microsoft.AspNetCore.Diagnostics;
using MyTemplate.Application.ApplicationManagement.Common.Constants;

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
                
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                var errMessage = GetErrorMessage(exceptionFeature);

                await context.Response.WriteAsJsonAsync(Result.WithFailure(Error.WithMessage(errMessage)));
            });
        });
    }

    private static string GetErrorMessage(IExceptionHandlerFeature? exception)
    {
        return exception?.Error switch
        {
            CustomException customEx => customEx.Message,
            UnauthorizedAccessException => CustomResponseMessages.UnAuthorized,
            _ => "Bilinmeyen bir hata meydana geldi"
        };
    }
}