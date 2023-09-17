using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Extensions
{
    public class AESCryptography
    {
        AesCryptoServiceProvider crypt_provider;
        private byte[] IVnKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        public AESCryptography()
        {
            crypt_provider = new AesCryptoServiceProvider();

            crypt_provider.BlockSize = 128;
            crypt_provider.KeySize = 256;
            crypt_provider.IV = IVnKey;
            crypt_provider.Key = IVnKey;
            crypt_provider.Mode = CipherMode.CBC;
            crypt_provider.Padding = PaddingMode.PKCS7;
        }

        public String EncryptAES(String PlainText)
        {
            if (PlainText == "") return "";
            ICryptoTransform transform = crypt_provider.CreateEncryptor();
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(PlainText), 0, PlainText.Length);

            string result = Convert.ToBase64String(encrypted_bytes);
            return result;
        }
        public String DecryptAES(String ChiperText)
        {
            if (ChiperText == "") return "";
            ICryptoTransform transform = crypt_provider.CreateDecryptor();
            byte[] enc_byte = Convert.FromBase64String(ChiperText);

            byte[] decrypted_bytes = transform.TransformFinalBlock(enc_byte, 0, enc_byte.Length);

            string result = ASCIIEncoding.ASCII.GetString(decrypted_bytes);
            return result;
        }

    }
}
