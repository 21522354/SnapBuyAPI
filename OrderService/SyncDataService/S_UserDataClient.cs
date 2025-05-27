using Newtonsoft.Json;
using OrderService.Common;
using OrderService.Models.Dtos.ResponseModels;

namespace OrderService.SyncDataService
{
    public interface IS_UserDataClient
    {
        Task<MRes_User> GetUserById(Guid id);
    }
    public class S_UserDataClient : IS_UserDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public S_UserDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<MRes_User> GetUserById(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_configuration["UserServiceEndpoint"]}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch user. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ResponseData<MRes_User>>(content);

            if (apiResponse == null || apiResponse.data == null)
            {
                throw new Exception("Invalid response structure or data is null.");
            }

            return apiResponse.data;
        }
    }
}
