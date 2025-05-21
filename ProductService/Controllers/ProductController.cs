using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IS_Product _s_Product;

        public ProductController(IS_Product s_Product)
        {
            _s_Product = s_Product;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_Product request)
        {
            var res = await _s_Product.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_Product request)
        {
            var res = await _s_Product.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_Product.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_Product.GetById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await _s_Product.GetList();
            return Ok(res);
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetListBySellerId(Guid sellerId)
        {
            var res = await _s_Product.GetListBySellerId(sellerId);
            return Ok(res);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetListByCategoryId(int categoryId)
        {
            var res = await _s_Product.GetListByCategoryId(categoryId);
            return Ok(res);
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> GetListProductStringForRecommend()
        {
            var res = await _s_Product.GetListProductStringForRecommend();
            return Ok(res);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var res = await _s_Product.GetUser(userId);
            return Ok(res);
        }

        [HttpGet("filter/{name}/{startPrice}/{endPrice}/{categoryName}/{tag}")]
        public async Task<IActionResult> GetListByFullParams(string name, decimal? startPrice, decimal? endPrice, string categoryName, string tag)
        {
            var res = await _s_Product.GetListByFullParams(name, startPrice, endPrice, categoryName, tag);
            return Ok(res);
        }
    }
}
