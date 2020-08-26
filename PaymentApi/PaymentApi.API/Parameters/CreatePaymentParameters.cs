namespace PaymentApi.API.Parameters
{
    using FluentValidation;

    public class CreatePaymentParameters
    {
        public ulong CardNumber { get; set; }

        public short CardExpirationMonth { get; set; }

        public short CardExpirationYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
        
        public short CVV { get; set; }
    }

    public class CreatePaymentParametersValidator : AbstractValidator<CreatePaymentParameters>
    {
        public CreatePaymentParametersValidator()
        {
            RuleFor(p => p.CardNumber).NotEmpty();
            RuleFor(p => p.CardExpirationMonth).InclusiveBetween((short)1, (short)12);
            RuleFor(p => p.CardExpirationYear).InclusiveBetween((short)1, (short)9999);
            RuleFor(p => p.CardExpirationYear).NotEmpty();
            RuleFor(p => p.CVV).InclusiveBetween((short)1, (short)999);
            RuleFor(p => p.Amount).GreaterThan(0);
            RuleFor(p => p.Currency).Length(3);
        }
    }
}
