using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Models.Dtos.ResponseModels;
using OrderService.Models.Entities;
using OrderService.Ultils;

namespace OrderService.Service
{
    public interface IS_Voucher
    {
        Task<ResponseData<MRes_Voucher>> Create(MReq_Voucher request);
        Task<ResponseData<MRes_Voucher>> Update(MReq_Voucher request);
        Task<ResponseData<int>> Delete(int id);
        Task<ResponseData<MRes_Voucher>> GetVoucherById(int id);
        Task<ResponseData<List<MRes_Voucher>>> GetAllVoucher();
        Task<ResponseData<List<MRes_Voucher>>> GetAvailableVoucherForUser(Guid userId, decimal orderTotal);
    }
    public class S_Voucher : IS_Voucher
    {
        private readonly OrderDBContext _context;
        private readonly IMapper _mapper;

        public S_Voucher(OrderDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseData<MRes_Voucher>> Create(MReq_Voucher request)
        {
            var res = new ResponseData<MRes_Voucher>();
            try
            {
                var data = new Voucher();
                _mapper.Map(request, data);
                data.Code = await GenerateVoucherCodeAsync();
                data.CreatedAt = DateTime.Now;
                _context.Vouchers.Add(data);

                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    res.error.code = 400;
                    return res;
                }
                var getById = await GetVoucherById(data.Id);
                res.result = 1;
                res.data = getById.data;
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

        public async Task<ResponseData<MRes_Voucher>> Update(MReq_Voucher request)
        {
            var res = new ResponseData<MRes_Voucher>();
            try
            {
                var data = await _context.Vouchers.FindAsync(request.Id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.Value = request.Value;
                data.MinOrderValue = request.MinOrderValue;
                data.ExpiryDate = request.ExpiryDate;
                _context.Vouchers.Update(data);

                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    res.error.code = 400;
                    return res;
                }
                var getById = await GetVoucherById(data.Id);
                res.result = 1;
                res.data = getById.data;
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

        public async Task<ResponseData<int>> Delete(int id)
        {
            var res = new ResponseData<int>();
            try
            {
                var data = await _context.Vouchers.FindAsync(id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.Vouchers.Remove(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
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

        public async Task<ResponseData<MRes_Voucher>> GetVoucherById(int id)
        {
            var res = new ResponseData<MRes_Voucher>();
            try
            {
                var data = await _context.Vouchers.FindAsync(id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_Voucher>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Voucher>>> GetAllVoucher()
        {
            var res = new ResponseData<List<MRes_Voucher>>();
            try
            {
                var data = await _context.Vouchers.ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Voucher>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Voucher>>> GetAvailableVoucherForUser(Guid userId, decimal orderTotal)
        {
            var res = new ResponseData<List<MRes_Voucher>>();
            try
            {
                var data = await _context.Vouchers
                    .Include(x => x.VoucherUsages)
                    .Where(x => x.ExpiryDate >= DateTime.Now && !x.VoucherUsages.Any(x => x.UserId == userId))
                    .ToListAsync();
                var returnData = _mapper.Map<List<MRes_Voucher>>(data);
                foreach (var item in returnData)
                {
                    if(item.MinOrderValue >= orderTotal)
                    {
                        item.CanUse = true;
                    }
                }
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

        public async Task<string> GenerateVoucherCodeAsync()
        {
            string today = DateTime.UtcNow.ToString("yyyyMMdd");
            string prefix = "VCH";

            // Lấy danh sách các mã order trong ngày hôm nay
            var todayVouchers = await _context.Vouchers
                .Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date)
                .OrderByDescending(o => o.Code)
                .Select(o => o.Code)
                .ToListAsync();

            int newNumber = 1;

            if (todayVouchers.Any())
            {
                // Tìm số thứ tự cuối cùng
                var lastCode = todayVouchers.First(); // Đã sắp xếp giảm dần rồi
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
