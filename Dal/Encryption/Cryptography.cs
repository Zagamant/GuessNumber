using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Dal.Encryption
{
    public static class Cryptography
    {
        private const string EncryptionKey = "0ram@hehehe"; //we can change the code converstion key as per our requirement    

        private static readonly Rfc2898DeriveBytes Pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
        {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });

        public static string Encrypt(string encryptString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Pdb.GetBytes(32);
                encryptor.IV = Pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }

            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Pdb.GetBytes(32);
                encryptor.IV = Pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        public static string EncryptSHA1(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input + "Tss...they doesn't know))"));

                //Use "X2" to UpperCase encrypt or "x2" to lowerCase
                return string.Concat(hash.Select(b => b.ToString("X2")));
            }
        }
    }
}
