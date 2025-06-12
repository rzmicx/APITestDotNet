using System.Security.Cryptography;
using System.Text;

namespace APITestDotNet.Services
{
    public static class AESHelper
    {
       
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("testmichaelmartin1234567"); 
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("backendapi123456");         

        public static byte[] Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Close();
            return ms.ToArray();
        }

        public static string Decrypt(byte[] cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
