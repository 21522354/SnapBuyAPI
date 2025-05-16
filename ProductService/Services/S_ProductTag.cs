using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;

namespace ProductService.Services
{
    public interface IS_ProductTag
    {
        Task<ResponseData<MRes_ProductTag>> Create(MReq_ProductTag request);

        Task<ResponseData<MRes_ProductTag>> Update(MReq_ProductTag request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_ProductTag>> GetById(int id);

        Task<ResponseData<List<MRes_ProductTag>>> GetList();
    }
    public class S_ProductTag : IS_ProductTag
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_ProductTag(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_ProductTag>> Create(MReq_ProductTag request)
        {
            var res = new ResponseData<MRes_ProductTag>();
            try
            {
                var data = new ProductTag();
                _mapper.Map(request, data);
                _context.ProductTags.Add(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                }
                var getById = await GetById(data.Id);
                res.result = 1;
                res.data = _mapper.Map<MRes_ProductTag>(data);
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
        public async Task<ResponseData<MRes_ProductTag>> Update(MReq_ProductTag request)
        {
            var res = new ResponseData<MRes_ProductTag>();
            try
            {
                var data = await _context.ProductTags.FindAsync(request.Id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _mapper.Map(request, data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                }
                var getById = await GetById(data.Id);
                res.result = 1;
                res.data = _mapper.Map<MRes_ProductTag>(data);
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
                var data = await _context.ProductTags.FindAsync(id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.ProductTags.Remove(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
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

        public async Task<ResponseData<MRes_ProductTag>> GetById(int id)
        {
            var res = new ResponseData<MRes_ProductTag>();
            try
            {
                var data = await _context.ProductTags.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var mapData = _mapper.Map<MRes_ProductTag>(data);
                res.data = mapData;
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductTag>>> GetList()
        {
            var res = new ResponseData<List<MRes_ProductTag>>();
            try
            {
                var data = await _context.ProductTags.AsNoTracking().ToListAsync();
                res.data = _mapper.Map<List<MRes_ProductTag>>(data);
                res.result = 1;
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
