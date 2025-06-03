using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Common;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Models.Dtos.ResponseModels;
using OrderService.Models.Entities;
using OrderService.SyncDataService;
using OrderService.Ultils;

namespace OrderService.Service
{
    public interface IS_Order
    {
        Task<ResponseData<MRes_Order>> Create(MReq_Order request);

        Task<ResponseData<int>> Delete(string id);

        Task<ResponseData<MRes_Order>> UpdateStatus(string id, string status);

        Task<ResponseData<int>> UpdateProductReviewed(int id);

        Task<ResponseData<MRes_Order>> GetOrderById(string id);

        Task<ResponseData<MRes_SellerStats>> GetSellersStats(Guid sellerId);

        Task<ResponseData<MRes_BuyerStats>> GetBuyerStats(Guid buyerId);

        Task<ResponseData<List<MRes_Order>>> GetListOrderByStatus(string status);

        Task<ResponseData<List<MRes_Order>>> GetListOrderForSeller(Guid sellerId);

        Task<ResponseData<List<MRes_Order>>> GetListOrderForBuyer(Guid buyerId);

        Task<ResponseData<List<MRes_OrderItem>>> GetListUnReviewedProduct(Guid buyerId);

        Task<ResponseData<List<MRes_Order>>> GetAllOrder();

        Task<ResponseData<MRes_SellerRevenue>> GetSellerRevenue(Guid sellerId, int type);
    }
    public class S_Order : IS_Order
    {
        private readonly OrderDBContext _context;
        private readonly IS_UserDataClient _s_UserDataClient;
        private readonly IS_ProductDataClient _s_ProductDataClient;
        private readonly IMapper _mapper;

        public S_Order(OrderDBContext context, IS_UserDataClient s_UserDataClient, IS_ProductDataClient s_ProductDataClient, IMapper mapper)
        {
            _context = context;
            _s_UserDataClient = s_UserDataClient;
            _s_ProductDataClient = s_ProductDataClient;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_Order>> Create(MReq_Order request)
        {
            var res = new ResponseData<MRes_Order>();
            try
            {
                var data = new Order();
                _mapper.Map(request, data);
                data.Id = await GenerateOrderCodeAsync();
                data.Status = "Pending";
                data.CreatedAt = DateTime.Now;

                _context.Orders.Add(data);

                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    res.error.code = 400;
                    return res;
                }
                var getById = await GetOrderById(data.Id);
                res.result = 1;
                res.data = getById.data;
                res.error.message = MessageErrorConstants.CREATE_SUCCESS;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_Order>> UpdateStatus(string id, string status)
        {
            var res = new ResponseData<MRes_Order>();
            try
            {
                var data = await _context.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.Status = status;
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    res.error.code = 400;
                    return res;
                }
                var getById = await GetOrderById(id);
                res.result = 1;
                res.data = getById.data;
                res.error.message = MessageErrorConstants.UPDATE_SUCCESS;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<int>> UpdateProductReviewed(int id)
        {
            var res = new ResponseData<int>();
            try
            {
                var productItem = await _context.SubOrderItems.FindAsync(id);
                if(productItem == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                productItem.IsReviewed = true;
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    res.error.code = 400;
                }
                res.result = 1;
                res.data = save;
                res.error.message = MessageErrorConstants.UPDATE_SUCCESS;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<int>> Delete(string id)
        {
            var res = new ResponseData<int>();
            try
            {
                var data = await _context.Orders.FindAsync(id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var orderItem = await _context.SubOrderItems.Where(x => x.OrderId == id).ToListAsync();
                _context.SubOrderItems.RemoveRange(orderItem);
                _context.Orders.Remove(data);

                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    res.error.code = 400;
                    return res;
                }
                res.result = 1;
                res.data = save;
                res.error.message = MessageErrorConstants.DELETE_SUCCESS;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_Order>> GetOrderById(string id)
        {
            var res = new ResponseData<MRes_Order>();
            try
            {
                var data = await _context.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                var seller = await _s_UserDataClient.GetUserById(data.SellerId);
                var buyer = await _s_UserDataClient.GetUserById(data.BuyerId);
                res.data = _mapper.Map<MRes_Order>(data);
                res.data.Seller = seller;
                res.data.Buyer = buyer;
                res.data.OrderItems = _mapper.Map<List<MRes_OrderItem>>(data.OrderItems);
                res.error.code = 201;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Order>>> GetListOrderByStatus(string status)
        {
            var res = new ResponseData<List<MRes_Order>>();
            try
            {
                var data = await _context.Orders.Include(x => x.OrderItems).Where(x => x.Status.ToLower().Trim().Equals(status.ToLower().Trim())).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Order>>(data);
                for(int i = 0; i < data.Count; i++)
                {
                    res.data[i].OrderItems = _mapper.Map<List<MRes_OrderItem>>(data[i].OrderItems);
                }
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Order>>> GetListOrderForSeller(Guid sellerId)
        {
            var res = new ResponseData<List<MRes_Order>>();
            try
            {
                var data = await _context.Orders.Where(x => x.SellerId == sellerId).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Order>>(data);
                foreach (var item in res.data)
                {
                    var buyer = await _s_UserDataClient.GetUserById(item.BuyerId);
                    if(buyer != null)
                    {
                        item.Buyer = buyer;
                    }
                }
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Order>>> GetListOrderForBuyer(Guid buyerId)
        {
            var res = new ResponseData<List<MRes_Order>>();
            try
            {
                var data = await _context.Orders.Where(x => x.BuyerId == buyerId).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Order>>(data);
                foreach (var item in res.data)
                {
                    var seller = await _s_UserDataClient.GetUserById(item.SellerId);
                    if (seller != null)
                    {
                        item.Seller = seller;
                    }
                }
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_SellerStats>> GetSellersStats(Guid sellerId)
        {
            var res = new ResponseData<MRes_SellerStats>();
            try
            {
                var data = new MRes_SellerStats();
                var products = await _s_ProductDataClient.GetProductBySeller(sellerId);
                var orders = await _context.Orders.Where(x => x.SellerId == sellerId).ToListAsync();
                data.ProductCount = products.Count;
                data.TotalPurchases = orders.Count;
                data.TotalRevenue = _context.Orders.Where(x => x.Status == "Paid").Sum(x => x.TotalAmount);

                res.result = 1;
                res.data = data;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_BuyerStats>> GetBuyerStats(Guid buyerId)
        {
            var res = new ResponseData<MRes_BuyerStats>();
            try
            {
                var data = new MRes_BuyerStats();
                var totalOrder = await _context.Orders.Where(x => x.BuyerId == buyerId).ToListAsync();
                var totalOrderIds = totalOrder.Select(x => x.Id);
                var purchaseCount = await _context.SubOrderItems.Where(x => totalOrderIds.Contains(x.OrderId)).ToListAsync();
                data.PurchaseCount = purchaseCount.Count;
                data.TotalSpent = totalOrder.Sum(x => x.TotalAmount);
                data.TotalOrders = totalOrder.Count;

                res.result = 1;
                res.data = data;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;

        }

        public async Task<ResponseData<List<MRes_OrderItem>>> GetListUnReviewedProduct(Guid buyerId)
        {
            var res = new ResponseData<List<MRes_OrderItem>>();
            try
            {
                var data = await _context.SubOrderItems
                    .Include(x => x.Order)
                    .Where(x => x.Order.BuyerId == buyerId && x.Order.Status == "Success" && x.IsReviewed == false).ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_OrderItem>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Order>>> GetAllOrder()
        {
            var res = new ResponseData<List<MRes_Order>>();
            try
            {
                var data = await _context.Orders.AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Order>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<string> GenerateOrderCodeAsync()
        {
            string today = DateTime.UtcNow.ToString("yyyyMMdd");
            string prefix = "ORD";

            // Lấy danh sách các mã order trong ngày hôm nay
            var todayOrders = await _context.Orders
                .Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date)
                .OrderByDescending(o => o.Id)
                .Select(o => o.Id)
                .ToListAsync();

            int newNumber = 1;

            if (todayOrders.Any())
            {
                // Tìm số thứ tự cuối cùng
                var lastCode = todayOrders.First(); // Đã sắp xếp giảm dần rồi
                var parts = lastCode.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                {
                    newNumber = lastNumber + 1;
                }
            }

            string newCode = $"{prefix}-{today}-{newNumber.ToString("D4")}";
            return newCode;
        }

        public async Task<ResponseData<MRes_SellerRevenue>> GetSellerRevenue(Guid sellerId, int type)
        {
            var res = new ResponseData<MRes_SellerRevenue>();
            try
            {
                var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.SellerId == sellerId && (x.Status == "Success" || x.Status == "Approved")).ToListAsync();
                var now = DateTime.Now;

                if (type == 1)
                {
                    // Tuần hiện tại
                    var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                    var endOfWeek = startOfWeek.AddDays(7);

                    orders = orders.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt < endOfWeek).ToList();
                }
                else if (type == 2)
                {
                    // Tháng hiện tại
                    var startOfMonth = new DateTime(now.Year, now.Month, 1);
                    var startOfNextMonth = startOfMonth.AddMonths(1);

                    orders = orders.Where(x => x.CreatedAt >= startOfMonth && x.CreatedAt < startOfNextMonth).ToList();
                }
                else if (type == 3)
                {
                    // Năm hiện tại
                    var startOfYear = new DateTime(now.Year, 1, 1);
                    var startOfNextYear = startOfYear.AddYears(1);

                    orders = orders.Where(x => x.CreatedAt >= startOfYear && x.CreatedAt < startOfNextYear).ToList();
                }

                int totalOrders = orders.Count;
                int itemSold = orders.Sum(x => x.OrderItems.Count);
                decimal totalRevenue = orders.Sum(x => x.TotalAmount);

                var returnData = new MRes_SellerRevenue
                {
                    TotalOrder = totalOrders,
                    ItemSold = itemSold,
                    Revenue = totalRevenue
                };

                res.result = 1;
                res.data = returnData;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;

        }
    }
}