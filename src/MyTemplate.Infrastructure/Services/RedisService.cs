using Common.Utilities;
using Microsoft.Extensions.Configuration;
using MyTemplate.Application.ApplicationManagement.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MyTemplate.Infrastructure.Services;

public class RedisService : ICacheService
{
    public IDatabase Db { get; set; }

    public RedisService(IConfiguration configuration)
    {
        Db = ConnectionMultiplexer.Connect($"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}").GetDatabase(0);
    }

    public async Task<T?> TryGetAsync<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return default;
        }

        if (!await KeyExistsAsync(key))
        {
            return default;
        }

        var value = await Db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            return default;
        }

        try
        {
            return CacheUtilities.Deserialize<T>(value!);
        }
        catch
        {
            await RemoveAsync(key);

            return default;
        }
    }

    public async Task<bool> TrySetAsync(string key, object data, TimeSpan? expiry = null)
    {
        try
        {
            return await Db.StringSetAsync(key, CacheUtilities.Serialize(data), expiry);
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await Db.KeyExistsAsync(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await Db.KeyDeleteAsync(key);
    }

    #region HashSet

    public async Task<T?> TryHashGetAsync<T>(string key, string field)
    {
        if (!await KeyExistsHashSetAsync(key, field))
        {
            return default;
        }

        var expiryData = await Db.HashGetAsync(key, $"{field}:expiry");

        if (!expiryData.IsNull) // Bu field için bir süre belirtilmişse
        {
            var expiryDate = JsonConvert.DeserializeObject<DateTime>(expiryData!);

            if (DateTime.UtcNow > expiryDate) // süre dolmuşsa
            {
                await RemoveHashSetAsync(key, field);
                await RemoveHashSetAsync(key, $"{field}:expiry");
                return default;
            }
        }
        var data = await Db.HashGetAsync(key, field);

        if (data.IsNullOrEmpty)
        {
            await RemoveHashSetAsync(key, field);
            return default;
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(data!);
        }
        catch
        {
            await RemoveHashSetAsync(key, field);
            return default;
        }
    }

    public async Task<bool> TryHashSetAsync(string key, string field, object data, TimeSpan? expiry = null)
    {
        try
        {
            if (expiry.HasValue)
            {
                await Db.HashSetAsync(key, $"{field}:expiry", JsonConvert.SerializeObject(DateTime.UtcNow.Add(expiry.Value)));
            }

            return await Db.HashSetAsync(key, field, CacheUtilities.Serialize(data));
        }
        catch
        {
            return await Task.FromResult(false);
        }
    }

    public Task<bool> RemoveHashSetAsync(string key, string field)
    {
        return Db.HashDeleteAsync(key, field);
    }

    public Task<bool> KeyExistsHashSetAsync(string key, string field)
    {
        return Db.HashExistsAsync(key, field);
    }

    #endregion HashSet
}