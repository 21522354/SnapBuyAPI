using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;

namespace ProductService.Services
{
    public interface IS_Product
    {
        Task<ResponseData<MRes_Product>> Create(MReq_Product request);

        Task<ResponseData<MRes_Product>> Update(MReq_Product request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_Product>> GetById(int id);

        Task<ResponseData<List<MRes_Product>>> GetList();

        Task<ResponseData<List<MRes_Product>>> GetListBySellerId(Guid sellerId);
    }
    public class S_Product : IS_Product
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_Product(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_Product>> Create(MReq_Product request)
        {
            var res = new ResponseData<MRes_Product>();
            try
            {
                var data = new Product();
                data.SellerId = request.SellerId;
                data.Name = request.Name;
                data.Description = request.Description;
                data.BasePrice = request.BasePrice;
                data.Status = request.Status;
                data.CategoryId = request.CategoryId;
                data.Quantity = request.Quantity;
                data.CreatedAt = DateTime.Now;

                _context.Products.Add(data);
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }

                var getById = await GetById(data.Id);
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

        public async Task<ResponseData<MRes_Product>> Update(MReq_Product request)
        {
            var res = new ResponseData<MRes_Product>();
            try
            {
                var data = await _context.Products.FindAsync(request.Id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.SellerId = request.SellerId;
                data.Name = request.Name;
                data.Description = request.Description;
                data.BasePrice = request.BasePrice;
                data.Status = request.Status;
                data.CategoryId = request.CategoryId;
                data.Quantity = request.Quantity;
                data.CreatedAt = DateTime.Now;

                _context.Products.Add(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }

                var getById = await GetById(data.Id);
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

        public async Task<ResponseData<int>> Delete(int id)
        {
            var res = new ResponseData<int>();
            try
            {
                var data = await _context.Products.FindAsync(id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.Products.Remove(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_DELETE;
                    return res;
                }
                res.result = 1;
                res.data = save;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_Product>> GetById(int id)
        {
            var res = new ResponseData<MRes_Product>();
            try
            {
                var data = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_Product>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Product>>> GetList()
        {
            var res = new ResponseData<List<MRes_Product>>();
            try
            {
                var data = await _context.Products.AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Product>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_Product>>> GetListBySellerId(Guid sellerId)
        {
            var res = new ResponseData<List<MRes_Product>>();
            try
            {
                var data = await _context.Products.Where(x => x.SellerId == sellerId).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_Product>>(data);
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
