namespace MyTemplate.Application.ApplicationManagement.Services;

public interface ICacheService
{
    public Task<T?> TryGetAsync<T>(string key);

    public Task<bool> TrySetAsync(string key, object data, TimeSpan? expiry = null);

    public Task<bool> KeyExistsAsync(string key);

    public Task<bool> RemoveAsync(string key);

    #region HashSet

    public Task<T?> TryHashGetAsync<T>(string key, string field);

    public Task<bool> TryHashSetAsync(string key, string field, object data, TimeSpan? expiry = null);

    public Task<bool> KeyExistsHashSetAsync(string key, string field);

    public Task<bool> RemoveHashSetAsync(string key, string field);

    #endregion HashSet
}