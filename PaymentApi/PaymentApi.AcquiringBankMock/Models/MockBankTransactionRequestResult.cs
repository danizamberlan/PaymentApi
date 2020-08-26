namespace PaymentApi.AcquiringBankMock.Models
{
    using System;
    public class MockBankTransactionRequestResult
    {
        public string StatusCode { get; set; }

        public Guid TransactionId { get; set; }
    }
}