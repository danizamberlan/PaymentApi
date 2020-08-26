namespace PaymentApi.Gateway.Client
{
    using System;
    using System.Threading.Tasks;
    using CrossCutting.HttpService;
    using Dto;

    public class AcquiringBankClient : IAcquiringBankClient
    {
        private readonly IHttpService httpService;
        private readonly string serviceUrl;

        public AcquiringBankClient(
            IHttpService httpService,
            string serviceUrl)
        {
            this.httpService = httpService;
            this.serviceUrl = serviceUrl;
        }

        public async Task<AcquiringBankTransactionRequestResult> SubmitPayment(AcquiringBankTransactionRequestParameters parameters)
        {
            string path = @"payments";

            var response = 
                await httpService.PostAsync<
                    AcquiringBankTransactionRequestParameters,
                    AcquiringBankTransactionRequestResult> (
                       serviceUrl,
                       path,
                       parameters);

            return response;
        }

        public async Task<AcquiringBankTransactionDetails> GetPaymentTransactionDetail(Guid transactionId)
        {
            string path = $@"payments/{transactionId}";

            var response =
                await httpService.GetAsync<AcquiringBankTransactionDetails>(
                    serviceUrl,
                    path);

            return response;
        }
    }
}
