namespace PaymentApi.CrossCutting.HttpService
{
    using System.Threading.Tasks;

    public interface IHttpService
    {
        Task<TResponse> GetAsync<TResponse>(
            string baseUrl,
            string path);

        Task<TResponse> PostAsync<TRequest, TResponse>(
            string baseUrl,
            string path,
            TRequest request);
    }
}