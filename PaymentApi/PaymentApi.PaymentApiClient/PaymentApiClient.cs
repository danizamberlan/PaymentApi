namespace PaymentApi.ApiClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Dto;

    /// <summary>
    /// Provides access to the PaymentApi operations.
    /// </summary>
    public class PaymentApiClient
    {
        private readonly HttpService httpService;
        private readonly string hostUrl;

        /// <summary>
        /// Creates a PaymentApiClient.
        /// </summary>
        /// <param name="httpClient">Instance of HttpClient.</param>
        /// <param name="hostUrl">PaymentApi host URL.</param>
        public PaymentApiClient(HttpClient httpClient, string hostUrl)
        {
            this.httpService = new HttpService(httpClient);
            this.hostUrl = hostUrl;
        }

        /// <summary>
        /// Creates a PaymentApiClient.
        /// </summary>
        /// <param name="hostUrl">PaymentApi host URL.</param>
        public PaymentApiClient(string hostUrl) : 
            this(new HttpClient(), hostUrl)
        {
        }

        /// <summary>
        /// Requests a new payment.
        /// </summary>
        /// <param name="parameters">Payment request parameters.</param>
        /// <returns>Unique identifier for the created payment request. In case of failure, returns null.</returns>
        public async Task<ApiResult<Guid>> CreatePaymentRequest(CreatePaymentParameters parameters)
        {
            string path = @"payment";

            var response = 
                await httpService.CreateAsync(
                   hostUrl,
                   path,
                   parameters);

            return response;
        }

        /// <summary>
        /// Retrieves details of an existing payment.
        /// </summary>
        /// <param name="transactionId">Payment unique identifier.</param>
        /// <returns>Payment details. In case of failure, returns null.</returns>
        public async Task<ApiResult<PaymentTransactionDetails>> GetPaymentTransactionDetail(Guid transactionId)
        {
            string path = $@"payment/{transactionId}";

            var response =
                await httpService.GetAsync<PaymentTransactionDetails>(
                    hostUrl,
                    path);

            return response;
        }
    }
}
