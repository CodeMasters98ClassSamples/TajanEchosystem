using Newtonsoft.Json;
using System.Text;
using Tajan.Standard.Application.Utils;

namespace Tajan.Standard.Application.Extensions;

public static class StringExtensions
{
    public static string AesEncrypt(this string s)
        => AesEncryption.EncryptString(s);

    public static string AesDecrypt(this string s)
        => AesEncryption.DecryptString(s);

    public static string Hash(this string s)
        => HashProvider.Hash(s);

    public static string Hash(this string s, string salt)
        => HashProvider.Hash(s, salt);

    public static bool IsValidIpAddress(this string s)
        => RegexValidator.IsValidIpAddress(s);

    public static bool IsPersianName(this string s)
        => RegexValidator.IsPersianName(s);

    public static bool IsLongPhoneNumber(this string s)
        => RegexValidator.IsValidLongPhoneNumber(s);

    public static bool IsValidEmail(this string s)
        => RegexValidator.IsValidEmail(s);

    public static byte[] GetBytesUtf8(this string s)
        => Encoding.UTF8.GetBytes(s);

    public static T Deserialize<T>(this string s)
        => JsonConvert.DeserializeObject<T>(s);

    public static bool DoesNotHaveValue(this string s)
        => !HasValue(s);

    public static bool HasValue(this string s)
        => s != null && s.Trim() != string.Empty;

    public static bool IsInt(this string s)
        => int.TryParse(s, out _);

    public static T ParseEnum<T>(this string s)
        where T : Enum
        => (T)Enum.Parse(typeof(T), s, true);

    public static string Mask(this string s, int take, int maskLength, char maskCharacter = '*')
        => $"{s[..take]} {new string(maskCharacter, s.Length <= take ? s.Length : maskLength)}";
}
