using CartService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IS_Cart _s_Cart;

        public CartController(IS_Cart s_Cart)
        {
            _s_Cart = s_Cart;
        }

        [HttpPost]
        public async Task<IActionResult> InsertProduct(Guid userId, int productId, string productName, string productImageUrl, string productNote, decimal productPrice, int quantity)
        {
            var res = await _s_Cart.InsertItem(userId, productId, productName, productImageUrl, productNote, productPrice, quantity);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(Guid userId, int productId, int quantity)
        {
            var res = await _s_Cart.UpdateQuantity(userId, productId, quantity);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var res = await _s_Cart.GetCartByUserId(userId);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteItem(Guid userId, int productId)
        {
            var res = await _s_Cart.DeleteItem(userId, productId);
            return Ok(res);
        }
    }
}
