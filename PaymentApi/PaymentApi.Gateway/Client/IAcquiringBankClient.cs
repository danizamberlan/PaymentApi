namespace PaymentApi.Gateway.Client
{
    using System;
    using System.Threading.Tasks;
    using Dto;

    public interface IAcquiringBankClient
    {
        Task<AcquiringBankTransactionRequestResult> SubmitPayment(AcquiringBankTransactionRequestParameters parameters);

        Task<AcquiringBankTransactionDetails> GetPaymentTransactionDetail(Guid transactionId);
    }
}