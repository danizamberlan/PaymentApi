namespace PaymentApi.Application.Commands.CreatePayment
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CrossCutting;
    using CrossCutting.Cqrs;
    using Domain;
    using Ether.Outcomes;
    using Gateway;
    using Microsoft.Extensions.Logging;

    public class CreatePaymentCommandHandler : ICreationCommandHandler<CreatePaymentCommandParameters>
    {
        private readonly IAcquiringBankGateway acquiringBankGateway;
        private readonly IEncryptionLibrary encryptionService;
        private readonly ILogger<CreatePaymentCommandHandler> logger;

        public CreatePaymentCommandHandler(
            IAcquiringBankGateway acquiringBankGateway,
            IEncryptionLibrary encryptionService,
            ILogger<CreatePaymentCommandHandler> logger)
        {
            this.acquiringBankGateway = acquiringBankGateway;
            this.encryptionService = encryptionService;
            this.logger = logger;
        }
        
        public async Task<IOutcome<Guid>> Handle(
            CreatePaymentCommandParameters request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var parameters = new BankTransactionRequestParameters
                {
                    Amount = request.Amount,
                    CardExpirationMonth = request.CardExpirationMonth,
                    CardExpirationYear = request.CardExpirationYear,
                    CardNumber = request.CardNumber,
                    Currency = request.Currency,
                    CVV = request.CVV
                };

                var submitPaymentResult =
                    await acquiringBankGateway.SubmitTransactionRequest(parameters);

                var encryptedId = encryptionService.EncryptGuid(submitPaymentResult.TransactionId);

                logger.LogInformation(
                    $@"Created transaction ""{submitPaymentResult.TransactionId}"" with status code {submitPaymentResult.StatusCode} and PaymentId ""{encryptedId}"".");

                return Outcomes.Success(encryptedId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not create payment. Please try again later.");
                return Outcomes.Failure<Guid>().WithMessage("Could not create payment. Please try again later.");
            }
        }
    }
}
