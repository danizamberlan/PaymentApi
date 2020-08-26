namespace PaymentApi.Gateway
{
    using System;
    using System.Threading.Tasks;
    using PaymentApi.Domain;

    public interface IAcquiringBankGateway
    {
        Task<BankTransactionRequestResult> SubmitTransactionRequest(BankTransactionRequestParameters transactionRequestParameters);

        Task<BankTransactionDetails> GetTransactionDetail(Guid transactionId);
    }
}
