namespace PaymentApi.Gateway
{
    using System;
    using System.Threading.Tasks;
    using Client;
    using Domain;
    using Dto;

    public class AcquiringBankGateway : IAcquiringBankGateway
    {
        private readonly IAcquiringBankClient client;

        public AcquiringBankGateway(IAcquiringBankClient client)
        {
            this.client = client;
        }

        public async Task<BankTransactionRequestResult> SubmitTransactionRequest(BankTransactionRequestParameters transactionRequestParameters)
        {
            var parameters = new AcquiringBankTransactionRequestParameters
            {
                Amount = transactionRequestParameters.Amount,
                CardExpirationMonth = transactionRequestParameters.CardExpirationMonth,
                CardExpirationYear = transactionRequestParameters.CardExpirationYear,
                CardNumber = transactionRequestParameters.CardNumber,
                Currency = transactionRequestParameters.Currency,
                CVV = transactionRequestParameters.CVV
            };

            var result = await client.SubmitPayment(parameters);

            var domainResult = new BankTransactionRequestResult()
            {
                StatusCode = result.StatusCode,
                TransactionId = result.TransactionId
            };

            return domainResult;
        }

        public async Task<BankTransactionDetails> GetTransactionDetail(Guid transactionId)
        {
            var result = await client.GetPaymentTransactionDetail(transactionId);

            if (result == null)
            {
                return null;
            }

            var domainResult = new BankTransactionDetails
            {
                Amount = result.Amount,
                CardHolder = result.CardHolder,
                CardNumber = result.CardNumber,
                Currency = result.Currency,
                PaymentStatus = result.PaymentStatus,
                TransactionId = transactionId
            };

            return domainResult;
        }
    }
}
