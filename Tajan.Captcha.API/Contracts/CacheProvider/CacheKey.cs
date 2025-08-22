namespace Tajan.Captcha.API.Contracts.CacheProvider;


public class CacheKey
{
    private const string GeneralKey = "X_";

    private CacheKey(string key)
        => Value = $"{GeneralKey}{key}";

    public string Value { get; private set; }

    public static CacheKey General(string key)
        => new($"{key}");

    public static CacheKey Otp(string key)
        => new($"OTP_{key}");

    public static CacheKey Captcha(string key)
        => new($"CAPTCHA_{key}");

    public static CacheKey DynamicSetting(string key)
        => new($"DynamicSetting_{key}");

}

