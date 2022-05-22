using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Decryptor.Core.Utilities;

#pragma warning disable CA1416 // Validate platform compatibility
public static class PasswordProtector
{
    private static readonly byte[] _entropy = Encoding.UTF8.GetBytes(":D(r>N7^&oCs;b^~3+[=33]NiakD7]J2[aEDlmIu`[hNRM/#KnbxCq:Nhps@!WW");

    public static SecureString DecryptString(string encryptedData)
    {
        try
        {
            byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData),
                                                           _entropy,
                                                           DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData).ToSecureString();
        }
        catch
        {
            return new SecureString();
        }
    }

    public static string GetEncryptedString(SecureString input)
    {
        byte[] encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(input.ToInsecureString()),
                                                     _entropy,
                                                     DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encryptedData);
    }
    public static string ToInsecureString(this SecureString input)
    {
        if (input == null)
            return string.Empty;
        IntPtr ptr = Marshal.SecureStringToBSTR(input);
        try
        {
            return Marshal.PtrToStringBSTR(ptr);
        }
        finally
        {
            Marshal.ZeroFreeBSTR(ptr);
        }
    }

    public static SecureString ToSecureString(this string input)
    {
        var secure = new SecureString();
        foreach (char c in input)
        {
            secure.AppendChar(c);
        }
        secure.MakeReadOnly();
        return secure;
    }
}
#pragma warning restore CA1416 // Validate platform compatibility
