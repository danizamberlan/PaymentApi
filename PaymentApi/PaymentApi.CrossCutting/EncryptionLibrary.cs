namespace PaymentApi.CrossCutting
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class EncryptionLibrary : IEncryptionLibrary
    {
        private readonly string key;

        public EncryptionLibrary(string key)
        {
            this.key = key;
        }
        
        public Guid EncryptGuid(Guid identifier)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Padding = PaddingMode.Zeros;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                array = encryptor.TransformFinalBlock(identifier.ToByteArray(), 0, 16);
            }

            return new Guid(array);
        }

        public Guid DecryptGuid(Guid cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = cipherText.ToByteArray();

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.Zeros;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                return new Guid(decryptor.TransformFinalBlock(buffer, 0, 16));
            }
        }
    }
}
