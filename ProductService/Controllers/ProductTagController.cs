using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/productTags")]
    [ApiController]
    public class ProductTagController : ControllerBase
    {
        private readonly IS_ProductTag _s_ProductTag;

        public ProductTagController(IS_ProductTag s_ProductTag)
        {
            _s_ProductTag = s_ProductTag;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_ProductTag request)
        {
            var res = await _s_ProductTag.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_ProductTag request)
        {
            var res = await _s_ProductTag.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_ProductTag.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_ProductTag.GetById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await _s_ProductTag.GetList();
            return Ok(res);
        }
    }
}
