namespace PaymentApi.Application.Queries.GetPaymentDetails
{
    using System;
    using CrossCutting.Cqrs;

    public class GetPaymentDetailsQueryParameters : IQuery<GetPaymentDetailsQueryResult>
    {
        public GetPaymentDetailsQueryParameters(Guid paymentId)
        {
            PaymentId = paymentId;
        }

        public Guid PaymentId { get; }
    }
}
