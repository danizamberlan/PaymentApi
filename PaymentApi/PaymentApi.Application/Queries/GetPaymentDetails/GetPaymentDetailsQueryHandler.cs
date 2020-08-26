namespace PaymentApi.Application.Queries.GetPaymentDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using CrossCutting;
    using CrossCutting.Cqrs;
    using Domain;
    using Ether.Outcomes;
    using Gateway;

    public class GetPaymentDetailsQueryHandler
        : IQueryHandler<GetPaymentDetailsQueryParameters, GetPaymentDetailsQueryResult>
    {
        private readonly IAcquiringBankGateway acquiringBankGateway;
        private readonly IEncryptionLibrary encryptionService;

        public GetPaymentDetailsQueryHandler(
            IAcquiringBankGateway acquiringBankGateway,
            IEncryptionLibrary encryptionService)
        {
            this.acquiringBankGateway = acquiringBankGateway;
            this.encryptionService = encryptionService;
        }

        public async Task<IOutcome<GetPaymentDetailsQueryResult>> Handle(
            GetPaymentDetailsQueryParameters request, 
            CancellationToken cancellationToken)
        {
            var transactionId = encryptionService.DecryptGuid(request.PaymentId);
            
            var transactionDetail = await acquiringBankGateway.GetTransactionDetail(transactionId);

            if (transactionDetail != null)
            {
                var paymentDetails = new PaymentDetails
                {
                    PaymentId = request.PaymentId,
                    Amount = transactionDetail.Amount,
                    CardHolder = transactionDetail.CardHolder,
                    CardNumber = transactionDetail.CardNumber,
                    Currency = transactionDetail.Currency,
                    PaymentStatus = transactionDetail.PaymentStatus
                };

                return Outcomes.Success(new GetPaymentDetailsQueryResult(paymentDetails));
            }
            else
            {
                return Outcomes.Failure<GetPaymentDetailsQueryResult>().WithStatusCode(404);
            }
        }
    }
}
