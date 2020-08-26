namespace PaymentApi.Gateway.Dto
{
    public class AcquiringBankTransactionDetails
    {
        public string TransactionId { get; set; }

        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string PaymentStatus { get; set; }
    }
}
