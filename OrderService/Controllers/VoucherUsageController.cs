using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Service;

namespace OrderService.Controllers
{
    [Route("api/voucherUsages")]
    [ApiController]
    public class VoucherUsageController : ControllerBase
    {
        private readonly IS_VoucherUsage _s_VoucherUsage;

        public VoucherUsageController(IS_VoucherUsage s_VoucherUsage)
        {
            _s_VoucherUsage = s_VoucherUsage;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_VoucherUsage request)
        {
            var res = await _s_VoucherUsage.Create(request);
            return Ok(res);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> RemoveUsage(string orderId)
        {
            var res = await _s_VoucherUsage.RemoveUsage(orderId);
            return Ok(res);
        }
    }
}
