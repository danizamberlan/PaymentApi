namespace PaymentApi.CrossCutting
{
    using System;

    public interface IEncryptionLibrary
    {
        Guid EncryptGuid(Guid identifier);

        Guid DecryptGuid(Guid cipherText);
    }
}