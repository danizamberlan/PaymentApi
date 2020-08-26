namespace PaymentApi.ApiClient
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Text.Json;
    using Dto;

    internal class HttpService
    {
        private readonly HttpClient httpClient;

        public HttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResult<TResponse>> GetAsync<TResponse>(
            string baseUrl,
            string path) 
        {
            try
            {
                string combinedUrl = $"{baseUrl}/{path}";

                var response = await httpClient.GetAsync(combinedUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResult<TResponse>.Failure($"Error retrieving data. Status Code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResponse>(
                    content,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return ApiResult<TResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult<TResponse>.Failure($"Unexpected behavior. Exception message: {ex.Message}");
            }
        }

        public async Task<ApiResult<Guid>> CreateAsync<TRequest>(
            string baseUrl,
            string path,
            TRequest request)
        {
            try
            {
                string combinedUrl = $"{baseUrl}/{path}";

                var requestContent = new StringContent(
                JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(combinedUrl, requestContent);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    return ApiResult<Guid>.Failure($"Error processing create request. Status Code: {response.StatusCode}");
                }

                string location = response.Headers.Location.Segments.Last();

                return ApiResult<Guid>.Success(new Guid(location));
            }
            catch (Exception ex)
            {
                return ApiResult<Guid>.Failure($"Unexpected behavior. Exception message: {ex.Message}");
            }
        }
    }
}
