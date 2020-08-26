namespace PaymentApi.Application.Queries.GetPaymentDetails
{
    using Domain;

    public class GetPaymentDetailsQueryResult
    {
        public GetPaymentDetailsQueryResult(PaymentDetails paymentDetail)
        {
            PaymentDetails = paymentDetail;
        }

        public PaymentDetails PaymentDetails { get; }
    }
}
