// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     XorCipher.cs
// Author:   Latency McLaughlin
// Date:     05/24/2026
// ****************************************************************************

using System.Text;

namespace AutoUpdaterDotNET.Extensions;

internal static class XorCipher
{
    extension(string text)
    {
        public string Encrypt(string key)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var keyBytes  = Encoding.UTF8.GetBytes(key);
            var result    = new byte[textBytes.Length];

            for (var i = 0; i < textBytes.Length; i++)
                // XOR each byte of the text with a byte from the key
                // The modulo (%) operator allows the key to repeat if it is shorter than the text
                result[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);

            // Convert the resulting bytes to a Base64 string for safe transport/storage
            return Convert.ToBase64String(result);
        }

        public string Decrypt(string key)
        {
            // Decode the Base64 string back into raw bytes
            var encryptedBytes = Convert.FromBase64String(text);
            var keyBytes       = Encoding.UTF8.GetBytes(key);
            var result         = new byte[encryptedBytes.Length];

            for (var i = 0; i < encryptedBytes.Length; i++)
                // XOR is reversible; applying the same key again decrypts the data
                result[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);

            return Encoding.UTF8.GetString(result);
        }
    }
}