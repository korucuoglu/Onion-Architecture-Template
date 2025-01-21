using AspNetCoreRateLimit;
using MyTemplate.API.Extensions;
using MyTemplate.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddTransient<IMiddleware, SecurityHeadersMiddleware>();

var app = builder.Build();

// IP Rate Limiting middleware
app.UseIpRateLimiting();

// Security Headers middleware
app.UseMiddleware<SecurityHeadersMiddleware>();

// Configure Swagger middleware
app.UseSwaggerServices(app.Environment);

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseHealtCheck();
app.UseGlobalExceptionHandler();
app.MapControllers();
app.Run();