namespace Tajan.Captcha.API.Contracts;

[AttributeUsage(AttributeTargets.All)]
public class ValueAttribute<T>(T value) : Attribute
{
    public T Value { get; init; } = value;
}
