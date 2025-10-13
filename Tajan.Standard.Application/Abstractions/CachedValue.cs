namespace Tajan.Standard.Application.Abstractions;

public record CachedValue
{
    public static CachedValue<T> Invalid<T>()
        => new(default, false);

    public static CachedValue<T> Valid<T>(T value)
        => new(value, true);
}

public record CachedValue<T>(T Value, bool IsValid)
{
    public static implicit operator T(CachedValue<T> cachedValue)
        => cachedValue.Value;
}
