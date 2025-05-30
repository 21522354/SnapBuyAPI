using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Common;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Service;

namespace OrderService.Controllers
{
    [Route("api/vouchers")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IS_Voucher _s_Voucher;

        public VoucherController(IS_Voucher s_Voucher)
        {
            _s_Voucher = s_Voucher;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_Voucher request)
        {
            var res = await _s_Voucher.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MReq_Voucher request)
        {
            var res = await _s_Voucher.Update(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_Voucher.Delete(id);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherById(int id)
        {
            var res = await _s_Voucher.GetVoucherById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVoucher()
        {
            var res = await _s_Voucher.GetAllVoucher();
            return Ok(res);
        }

        [HttpGet("{userId}/{orderTotal}")]
        public async Task<IActionResult> GetAvaGetAvailableVoucherForUser(Guid userId, decimal orderTotal)
        {
            var res = await _s_Voucher.GetAvailableVoucherForUser(userId, orderTotal);
            return Ok(res);
        }
    }
}
