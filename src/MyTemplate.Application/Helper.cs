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
    }
    public static T? Deserialize<T>(string value)
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
