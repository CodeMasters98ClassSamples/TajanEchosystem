namespace Tajan.Captcha.API.Contracts;

public interface IDynamicSettingService
{
    Task<T> Get<T>(string key, T defaultValue = default, CancellationToken ct = default);
    Task<T> Get<T>(DynamicSettingKey key, CancellationToken ct = default);

    Task Set<T>(string key, T value, CancellationToken ct = default);
    Task Set<T>(DynamicSettingKey key, T value, CancellationToken ct = default);

    Task ResetCache(CancellationToken ct = default);
    Task<object> GetAllCahcetSettings(CancellationToken ct = default);
}

