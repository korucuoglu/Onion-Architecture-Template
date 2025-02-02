using AspNetCoreRateLimit;
using Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerServices();

var app = builder.Build();

// IP Rate Limiting middleware
app.UseIpRateLimiting();

// Configure Swagger middleware
app.UseSwaggerServices(app.Environment);

// app.UseSecurityHeaders();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseHealtCheck();
app.UseGlobalExceptionHandler();
app.MapControllers();
app.Run();