using System.Security.Cryptography;
using System.Text;
using Tajan.Standard.Domain.Settings;

namespace Tajan.Standard.Application.Utils;

public static class HashProvider
{
    public static string Hash(string key, string salt)
    {
        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(salt))
        {
            var keyBytes = Encoding.Unicode.GetBytes(key);
            var saltBytes = Encoding.Unicode.GetBytes(salt);
            var array = new byte[keyBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, array, 0, saltBytes.Length);
            Buffer.BlockCopy(keyBytes, 0, array, saltBytes.Length, keyBytes.Length);
            return Convert.ToBase64String(SHA256.HashData(array));
        }
        else
        {
            return string.Empty;
        }
    }

    public static string Hash(string key)
        => Hash(key, HashSettings.DefaultSalt);
}
