using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SabiAsp.Encryption
{
    public class Encrypto
    {
        public static string EncryptString(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();

                aesAlg.Key = Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"]);
                aesAlg.IV = Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["EncryptionVector"]);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }

            }
            finally
            {
                // Clear the RijndaelManaged object.
                if ((aesAlg != null))
                {
                    aesAlg.Clear();
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(msEncrypt.ToArray());

        }

        public static string DecryptString(string value)
        {
            byte[] cipherText = Convert.FromBase64String(value);
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();

                aesAlg.Key = Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"]);
                aesAlg.IV = Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["EncryptionVector"]);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if ((aesAlg != null))
                {
                    aesAlg.Clear();
                }
            }
            return plaintext;
        }
    }
}