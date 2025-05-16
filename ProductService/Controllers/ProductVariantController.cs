using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/productVariants")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IS_ProductVariant _s_ProductVariant;

        public ProductVariantController(IS_ProductVariant s_ProductVariant)
        {
            _s_ProductVariant = s_ProductVariant;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_ProductVariant request)
        {
            var res = await _s_ProductVariant.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_ProductVariant request)
        {
            var res = await _s_ProductVariant.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_ProductVariant.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_ProductVariant.GetById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await _s_ProductVariant.GetList();
            return Ok(res);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetListByProductId(int id)
        {
            var res = await _s_ProductVariant.GetListByProduct(id);
            return Ok(res);
        }
    }
}
