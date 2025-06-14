﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Service;

namespace OrderService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IS_Order _s_Order;

        public OrderController(IS_Order s_Order)
        {
            _s_Order = s_Order;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MReq_Order request)
        {
            var res = await _s_Order.Create(request);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _s_Order.Delete(id);
            return Ok(res);
        }

        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(string id, string status)
        {
            var res = await _s_Order.UpdateStatus(id, status);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await _s_Order.GetOrderById(id);
            return Ok(res);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetListByStatus(string status)
        {
            var res = await _s_Order.GetListOrderByStatus(status);
            return Ok(res);
        }

        [HttpGet("buyer/{buyerId}")]
        public async Task<IActionResult> GetListByBuyer(Guid buyerId)
        {
            var res = await _s_Order.GetListOrderForBuyer(buyerId);
            return Ok(res);
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetListBySeller(Guid sellerId)
        {
            var res = await _s_Order.GetListOrderForSeller(sellerId);
            return Ok(res);
        }

        [HttpGet("buyer/stats/{buyerId}")]
        public async Task<IActionResult> GetBuyerStats(Guid buyerId)
        {
            var res = await _s_Order.GetBuyerStats(buyerId);
            return Ok(res);
        }

        [HttpGet("seller/stats/{sellerId}")]
        public async Task<IActionResult> GetSellerStats(Guid sellerId)
        {
            var res = await _s_Order.GetSellersStats(sellerId);
            return Ok(res);
        }

        [HttpPut("orderItems/{id}")]
        public async Task<IActionResult> UpdateProductReviewed(int id)
        {
            var res = await _s_Order.UpdateProductReviewed(id);
            return Ok(res);
        }

        [HttpGet("orderItems/unReviewed/{buyerId}")]
        public async Task<IActionResult> GetListProductUnReviewed(Guid buyerId)
        {
            var res = await _s_Order.GetListUnReviewedProduct(buyerId);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetALl()
        {
            var res = await _s_Order.GetAllOrder();
            return Ok(res);
        }

        [HttpGet("seller/revenue/{sellerId}/{type}")]
        public async Task<IActionResult> GetSellerRevenue(Guid sellerId, int type)
        {
            var res = await _s_Order.GetSellerRevenue(sellerId, type);
            return Ok(res);
        }
    }
}
