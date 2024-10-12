using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyTemplate.Application;
public static class Helper
{
    public static object? GetSettingValue(Setting? setting)
    {
        if (setting is null)
        {
            return null;
        }

        return setting.DataType switch
        {
            "long" => Deserialize<long>(setting.Value),
            "long[]" => Deserialize<long[]>(setting.Value),
            "int" => Deserialize<int>(setting.Value),
            "int[]" => Deserialize<int[]>(setting.Value),
            "string" => Deserialize<string>(setting.Value),
            "string[]" => Deserialize<string[]>(setting.Value),
            "boolean" => Deserialize<bool>(setting.Value),
            _ => throw new Exception("Hatalı ayar gönderildi")
        };

        static T? Deserialize<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch
            {
                return default;
            }
        }
    }
   
    public static string GetUserId(IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor?.HttpContext?.User?.FindFirstValue("id")
                 ?? throw new Exception("UserId değeri alınamadı");
    }
}
