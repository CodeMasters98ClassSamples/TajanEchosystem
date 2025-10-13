namespace Tajan.Standard.Domain.Abstratcions;

public class CacheKey
{
    private const string GeneralKey = "Tajan_";

    private CacheKey(string key)
        => Value = $"{GeneralKey}{key}";

    public string Value { get; private set; }


    public static CacheKey General(string key)
        => new($"{key}");

    public static CacheKey Otp(string key)
        => new($"OTP_{key}");

    public static CacheKey Captcha(string key)
        => new($"CAPTCHA_{key}");

    public static CacheKey Token(string key)
        => new($"TOKEN_{key}");

    public static CacheKey ConnectionTest()
        => new("CONNECTION_TEST");

    public static CacheKey Product(long id)
        => new($"PRODUCT_{id}");

}
