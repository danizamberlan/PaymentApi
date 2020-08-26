namespace PaymentApi.Domain
{
    using System;

    public class BankTransactionDetails
    {
        public Guid TransactionId { get; set; }

        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string PaymentStatus { get; set; }
    }
}
