namespace PaymentApi.Domain
{
    using System;

    public class BankTransactionRequestResult
    {
        public string StatusCode { get; set; }

        public Guid TransactionId { get; set; }
    }
}
