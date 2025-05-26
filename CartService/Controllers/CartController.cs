using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IS_Cart _s_Cart;

        public CartController(IS_Cart s_Cart)
        {
            _s_Cart = s_Cart;
        }

        [HttpPost]
        public async Task<IActionResult> InsertProduct(MReq_Cart request)
        {
            var res = await _s_Cart.InsertItem(request);
            return Ok(res);
        }

        [HttpPut("{userId}/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(Guid userId, int productId, int quantity)
        {
            var res = await _s_Cart.UpdateQuantity(userId, productId, quantity);
            return Ok(res);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var res = await _s_Cart.GetCartByUserId(userId);
            return Ok(res);
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> DeleteItem(Guid userId, int productId)
        {
            var res = await _s_Cart.DeleteItem(userId, productId);
            return Ok(res);
        }
    }
}
