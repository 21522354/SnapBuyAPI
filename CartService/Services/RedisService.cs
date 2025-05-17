using StackExchange.Redis;

namespace CartService.Services
{
    public class RedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        // Lưu dữ liệu vào Redis
        public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        // Lấy dữ liệu từ Redis
        public async Task<string?> GetValueAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        // Xóa key khỏi Redis
        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}
