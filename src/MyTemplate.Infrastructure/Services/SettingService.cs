using MyTemplate.Application.ApplicationManagement.Interfaces;
using MyTemplate.Domain.Entities;
using Newtonsoft.Json;

namespace MyTemplate.Infrastructure.Services;
public class SettingService : ISettingService
{
    private readonly IEnumerable<Setting> _settings;
    public SettingService(IEnumerable<Setting> settings)
    {
        _settings = settings;
    }

    public bool EmailConfirmRequired() => (bool)GetSettingValue("EmailConfirmRequired");

    private object? GetSettingValue(string key)
    {
        var setting = _settings.FirstOrDefault(x => x.Key == key);

        if (setting is null)
        {
            return default;
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
            _ => default
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
}
