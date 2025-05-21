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
        public Task<MRes_User> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
