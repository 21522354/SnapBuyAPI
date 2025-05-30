using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Models.Dtos.RequestModels;
using OrderService.Models.Dtos.ResponseModels;
using OrderService.Models.Entities;
using OrderService.Ultils;

namespace OrderService.Service
{
    public interface IS_VoucherUsage
    {
        Task<ResponseData<MRes_VoucherUsage>> Create(MReq_VoucherUsage request);
        Task<ResponseData<int>> RemoveUsage(string orderId);
    }
    public class S_VoucherUsage : IS_VoucherUsage
    {
        private readonly OrderDBContext _context;
        private readonly IMapper _mapper;

        public S_VoucherUsage(OrderDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseData<MRes_VoucherUsage>> Create(MReq_VoucherUsage request)
        {
            var res = new ResponseData<MRes_VoucherUsage>();
            try
            {
                var data = new VoucherUsage();
                _mapper.Map(request, data);
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(x => x.Code.ToLower().Trim().Equals(request.Code.ToLower().Trim()));
                if (voucher == null)
                {
                    res.error.message = "Voucher code is not exist";
                    return res;
                }
                data.VoucherId = voucher.Id;
                data.UsedAt = DateTime.Now;
                _context.VoucherUsages.Add(data);
                var save = await _context.SaveChangesAsync();

                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_VoucherUsage>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<int>> RemoveUsage(string orderId)
        {
            var res = new ResponseData<int>();
            try
            {
                var data = await _context.VoucherUsages.FirstOrDefaultAsync(x => x.OrderId.ToLower().Trim().Equals(orderId.ToLower().Trim()));
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.VoucherUsages.Remove(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
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
    }
}
