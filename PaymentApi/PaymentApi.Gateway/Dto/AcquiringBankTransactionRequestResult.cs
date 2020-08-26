namespace PaymentApi.Gateway.Dto
{
    using System;

    public class AcquiringBankTransactionRequestResult
    {
        public string StatusCode { get; set; }

        public Guid TransactionId { get; set; }
    }
}
