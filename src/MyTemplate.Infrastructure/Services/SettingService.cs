using Common.Entities.Setting;
using Common.Exceptions;
using MyTemplate.Application.ApplicationManagement.Services;
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

    private object GetSettingValue(string key)
    {
        var setting = _settings.First(x => x.Key == key);

        return setting.DataType switch
        {
            "long" => Deserialize<long>(setting.Value),
            "long[]" => Deserialize<long[]>(setting.Value),
            "int" => Deserialize<int>(setting.Value),
            "int[]" => Deserialize<int[]>(setting.Value),
            "string" => Deserialize<string>(setting.Value),
            "string[]" => Deserialize<string[]>(setting.Value),
            "boolean" => Deserialize<bool>(setting.Value),
            _ => throw new CustomException("Bilinmeyen bir veri tipi girildi.")
        };

        static T Deserialize<T>(string value)
        { 
            return JsonConvert.DeserializeObject<T>(value)!;
        }
    }
}