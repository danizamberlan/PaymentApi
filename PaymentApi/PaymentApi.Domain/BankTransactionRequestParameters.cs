namespace PaymentApi.Domain
{
    public class BankTransactionRequestParameters
    {
        public ulong CardNumber { get; set; }

        public short CardExpirationMonth { get; set; }

        public short CardExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public short CVV { get; set; }
    }
}
