using Newtonsoft.Json;
using OrderService.Common;
using OrderService.Models.Dtos.ResponseModels;

namespace OrderService.SyncDataService
{
    public interface IS_ProductDataClient
    {
        Task<List<MRes_Product>> GetProductBySeller(Guid sellerId);
    }
    public class S_ProductDataClient : IS_ProductDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public S_ProductDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<MRes_Product>> GetProductBySeller(Guid sellerId)
        {
            var response = await _httpClient.GetAsync($"{_configuration["ProductServiceEndpoint"]}/seller/{sellerId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch user. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ResponseData<List<MRes_Product>>>(content);

            if (apiResponse == null || apiResponse.data == null)
            {
                throw new Exception("Invalid response structure or data is null.");
            }

            return apiResponse.data;
        }
    }
}
