namespace PaymentApi.CrossCutting.HttpService
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class HttpService
        : IHttpService
    {
        private readonly HttpClient httpClient;

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(
            string baseUrl,
            string path) 
        {
            string combinedUrl = $"{baseUrl}/{path}";

            var response = await httpClient.GetAsync(combinedUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string baseUrl,
            string path,
            TRequest request)
        {
            string combinedUrl = $"{baseUrl}/{path}";

            var requestContent = new StringContent(
                JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(combinedUrl, requestContent);
            
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
