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
        var defaultMessage = "Bilinmeyen bir hata meydana geldi";

        if (exception is null)
        {
            return defaultMessage;
        }
        
        if (exception.Error is CustomException customEx)
        {
            return customEx.Message;
        }

        if (exception.Error is UnauthorizedAccessException)
        {
            return CustomResponseMessages.UnAuthorized;
        }
        
        return defaultMessage;
    }
}