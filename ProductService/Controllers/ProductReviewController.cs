using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/productReviews")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IS_ProductReview _s_ProductReview;

        public ProductReviewController(IS_ProductReview s_ProductReview)
        {
            _s_ProductReview = s_ProductReview;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_ProductReview request)
        {
            var res = await _s_ProductReview.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_ProductReview request)
        {
            var res = await _s_ProductReview.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_ProductReview.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_ProductReview.GetById(id);
            return Ok(res);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetListByProduct(int productId)
        {
            var res = await _s_ProductReview.GetListByProduct(productId);
            return Ok(res);
        }
    }
}
