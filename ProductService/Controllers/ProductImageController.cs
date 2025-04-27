using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IS_ProductImage _s_ProductImage;

        public ProductImageController(IS_ProductImage s_ProductImage)
        {
            _s_ProductImage = s_ProductImage;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_ProductImage request)
        {
            var res = await _s_ProductImage.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_ProductImage request)
        {
            var res = await _s_ProductImage.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_ProductImage.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_ProductImage.GetById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await _s_ProductImage.GetList();
            return Ok(res);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetListByProductId(int productId)
        {
            var res = await _s_ProductImage.GetListByProduct(productId);
            return Ok(res);
        }

    }
}
