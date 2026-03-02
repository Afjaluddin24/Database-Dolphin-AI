using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dolphin_AI.Helpers
{
    public static class PasswordCryptoHelper
    {
        // 32 bytes = AES-256
        private static readonly byte[] Key =
            Encoding.UTF8.GetBytes("1234567890abcdef1234567890abcdef");

        // 16 bytes = AES block size
        private static readonly byte[] IV =
            Encoding.UTF8.GetBytes("abcdef1234567890");

        public static string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);

            sw.Write(plainText);
            sw.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
