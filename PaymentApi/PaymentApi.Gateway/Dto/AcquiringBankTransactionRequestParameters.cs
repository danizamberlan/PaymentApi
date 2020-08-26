namespace PaymentApi.Gateway.Dto
{
    public class AcquiringBankTransactionRequestParameters
    {
        public ulong CardNumber { get; set; }

        public int CardExpirationMonth { get; set; }

        public int CardExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public short CVV { get; set; }
    }
}
