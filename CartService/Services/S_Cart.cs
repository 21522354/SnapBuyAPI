using CartService.Common;
using CartService.Models;
using Newtonsoft.Json;

namespace CartService.Services
{
    public interface IS_Cart
    {
        Task<ResponseData<Cart>> InsertItem(Guid userId, int productId, string productName, string productImageUrl, string productNote, decimal productPrice, int quantity);
        Task<ResponseData<Cart>> UpdateQuantity(Guid userId, int productId, int quantity);
        Task<ResponseData<Cart>> DeleteItem(Guid userId, int productid);
        Task<ResponseData<Cart>> GetCartByUserId(Guid userId);
    }
    public class S_Cart : IS_Cart
    {
        private readonly RedisService _redisService;
        private readonly string RedisPrefix = "cart_";

        public S_Cart(RedisService redisService)
        {
            _redisService = redisService;
        }

        private string GetCartKey(Guid userId) => $"{RedisPrefix}{userId}";

        public async Task<ResponseData<Cart>> InsertItem(Guid userId, int productId, string productName, string productImageUrl, string productNote, decimal productPrice, int quantity)
        {
            var res = new ResponseData<Cart>();
            try
            {
                var key = GetCartKey(userId);
                var cart = await GetOrCreateCartAsync(userId);

                var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == productId && x.ProductNote.Equals(productNote));
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    existingItem.ProductNote = productNote;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        ProductName = productName,
                        ProductImageUrl = productImageUrl,
                        ProductPrice = productPrice, // Bạn có thể lấy từ DB nếu cần
                        ProductNote = productNote,
                        Quantity = quantity
                    });
                }

                cart.UpdatedAt = DateTime.Now;

                await SaveCartAsync(key, cart);
                res.data = cart;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.message = ex.Message;
            }

            return res;
        }

        public async Task<ResponseData<Cart>> DeleteItem(Guid userId, int productId)
        {
            var res = new ResponseData<Cart>();
            try
            {
                var key = GetCartKey(userId);
                var cart = await GetOrCreateCartAsync(userId);

                cart.Items.RemoveAll(x => x.ProductId == productId);
                cart.UpdatedAt = DateTime.Now;

                await SaveCartAsync(key, cart);
                res.data = cart;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.message = ex.Message;
            }

            return res;
        }

        public async Task<ResponseData<Cart>> GetCartByUserId(Guid userId)
        {
            var res = new ResponseData<Cart>();
            try
            {
                var cart = await GetOrCreateCartAsync(userId);
                res.data = cart;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.message = ex.Message;
            }

            return res;
        }

        private async Task<Cart> GetOrCreateCartAsync(Guid userId)
        {
            var key = GetCartKey(userId);
            var redisValue = await _redisService.GetValueAsync(key);

            if (string.IsNullOrEmpty(redisValue))
            {
                return new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>(),
                    UpdatedAt = DateTime.Now
                };
            }

            return JsonConvert.DeserializeObject<Cart>(redisValue) ?? new Cart
            {
                UserId = userId,
                Items = new List<CartItem>(),
                UpdatedAt = DateTime.Now
            };
        }

        private async Task SaveCartAsync(string key, Cart cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            await _redisService.SetValueAsync(key, json, TimeSpan.FromDays(7)); // Cart giữ trong Redis 7 ngày
        }

        public async Task<ResponseData<Cart>> UpdateQuantity(Guid userId, int productId, int quantity)
        {
            var res = new ResponseData<Cart>();
            try
            {
                var key = GetCartKey(userId);
                var cart = await GetOrCreateCartAsync(userId);

                var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
                if (item == null)
                {
                    res.result = -1;
                    res.error.message = "Product not found in cart.";
                    return res;
                }

                if (quantity <= 0)
                {
                    // Nếu số lượng <= 0 thì xóa item khỏi cart
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                cart.UpdatedAt = DateTime.Now;
                await SaveCartAsync(key, cart);

                res.data = cart;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.message = ex.Message;
            }

            return res;
        }

    }
}
