using System.Text.RegularExpressions;

namespace Tajan.Standard.Application.Utils;

public static partial class RegexValidator
{
    [GeneratedRegex(@"^(25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})(\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})){3}$", RegexOptions.Compiled)]
    private static partial Regex IpAddressPattern();


    [GeneratedRegex(@"^9\d{9}$", RegexOptions.Compiled)]
    private static partial Regex LongPhoneNumberPattern();


    [GeneratedRegex(@"^[\u0600-\u06FF\s]+$", RegexOptions.Compiled)]
    private static partial Regex PersianNamePattern();

    [GeneratedRegex(@"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex EmailPattern();


    public static bool IsValidIpAddress(string ipAddress)
        => IpAddressPattern().IsMatch(ipAddress);

    public static bool IsValidLongPhoneNumber(string phoneNumber)
        => LongPhoneNumberPattern().IsMatch(phoneNumber);

    public static bool IsPersianName(string text)
        => PersianNamePattern().IsMatch(text);

    public static bool IsValidEmail(string email)
        => EmailPattern().IsMatch(email);
}
