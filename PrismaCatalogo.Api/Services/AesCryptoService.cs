using CloudinaryDotNet.Core;
using PrismaCatalogo.Api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PrismaCatalogo.Api.Services
{
    public class AesCryptoService : IAesCryptoService
    {
        public byte[] HexStringToBytes(string valor)
        {
            return Enumerable.Range(0, valor.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(valor.Substring(x, 2), 16))
                     .ToArray();
        }

        public string Encrypt (string valor, byte[] chave)
        {
            using var aes = Aes.Create();
            aes.Key = chave;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs, Encoding.UTF8))
            {
                sw.Write(valor);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt (string valor, byte[] chave)
        {
            byte[] bytes = Convert.FromBase64String(valor);

            using var aes = Aes.Create();
            aes.Key = chave;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            int ivSize = aes.BlockSize / 8;

            byte[] iv = new byte[ivSize];
            Array.Copy(bytes, 0, iv, 0, ivSize);
            aes.IV = iv;

            using var ms = new MemoryStream(bytes, iv.Length, bytes.Length - iv.Length); 
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            
            return sr.ReadToEnd();
        }
    }
}
