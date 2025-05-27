using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        Task<ResponseData<MRes_Order>> GetOrderById(string id);

        Task<ResponseData<List<MRes_Order>>> GetListOrderByStatus(string status);

        Task<ResponseData<List<MRes_Order>>> GetListOrderForSeller(Guid sellerId);

        Task<ResponseData<List<MRes_Order>>> GetListOrderForBuyer(Guid buyerId);
    }
    public class S_Order : IS_Order
    {
        private readonly OrderDBContext _context;
        private readonly IS_UserDataClient _s_UserDataClient;
        private readonly IMapper _mapper;

        public S_Order(OrderDBContext context, IS_UserDataClient s_UserDataClient, IMapper mapper)
        {
            _context = context;
            _s_UserDataClient = s_UserDataClient;
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

                var orderItems = request.OrderItems.Select(x => new OrderItem
                {
                    OrderId = data.Id,
                    ProductId = x.ProductId,
                    ProductImageUrl = x.ProductImageUrl,
                    ProductName = x.ProductName,
                    ProductNote = x.ProductNote,
                    ProductVariantId = x.ProductVariantId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                }).ToList();
                _context.SubOrderItems.AddRange(orderItems);

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
    }
}

