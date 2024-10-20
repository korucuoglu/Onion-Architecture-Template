using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mjml.Net;

namespace MyTemplate.Application.ApplicationManagement.Helpers;

public static class Helper
{
    public static string GetUserId(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.User?.FindFirstValue("id")
                 ?? throw new Exception("UserId değeri alınamadı");
    }

    public static async Task<string> GetHtmlTemplateAsync(CancellationToken cancellationToken = default, params string[] path)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine(path));

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("HTML dosyası bulunamadı.", templatePath);
        }

        var template = await File.ReadAllTextAsync(templatePath, cancellationToken);

        var extension = Path.GetExtension(templatePath);

        if (extension.Contains("html"))
        {
            return template;
        }

        if (extension.Contains("mjml"))
        {
            var mjmlRenderer = new MjmlRenderer();

            var result = mjmlRenderer.Render(template, new()
            {
                Beautify = false,
            });

            return result.Html;
        }

        throw new FileNotFoundException("İlgili dosyanın html veya mjml formatında olması gerekmektedir.");
    }

    public static T GetValueFromConfiguration<T>(IConfiguration configuration, string key, bool isRequired = true)
    {
        var value = configuration.GetValue<T>(key);

        if (value is null && isRequired)
        {
            throw new InvalidOperationException($"{key} ayarı yapılandırılmamış.");
        }

        return value!;
    }
}