using Newtonsoft.Json;

namespace MyTemplate.Application;
public static class Helper
{
    public static object? GetSettingValue(Setting setting)
    {
        return setting.DataType switch
        {
            "long" => Deserialize<long>(setting.Value),
            "long[]" => Deserialize<long[]>(setting.Value),
            "int" => Deserialize<int>(setting.Value),
            "int[]" => Deserialize<int[]>(setting.Value),
            "string" => Deserialize<string>(setting.Value),
            "string[]" => Deserialize<string[]>(setting.Value),
            "boolean" => Deserialize<bool>(setting.Value),
            _ => default
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
