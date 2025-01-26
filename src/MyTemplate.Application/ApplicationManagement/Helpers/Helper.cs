using Microsoft.Extensions.Configuration;
using Mjml.Net;

namespace MyTemplate.Application.ApplicationManagement.Helpers;

public static class Helper
{
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
}