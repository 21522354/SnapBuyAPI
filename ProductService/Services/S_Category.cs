using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;

namespace ProductService.Services
{
    public interface IS_Category
    {
        Task<ResponseData<MRes_Category>> Create(MReq_Category request);

        Task<ResponseData<MRes_Category>> Update(MReq_Category request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_Category>> GetById(int id);

        Task<ResponseData<List<MRes_Category>>> GetList();
    }
    public class S_Category : IS_Category
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_Category(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_Category>> Create(MReq_Category request)
        {
            var res = new ResponseData<MRes_Category>();
            try
            {
                var data = new Category();
                _mapper.Map(request, data);
                data.CreatedAt = DateTime.Now;
                _context.Categories.Add(data);
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                }
                var getById = await GetById(data.Id);
                res.result = 1;
                res.data = _mapper.Map<MRes_Category>(data);
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
        public async Task<ResponseData<MRes_Category>> Update(MReq_Category request)
        {
            var res = new ResponseData<MRes_Category>();
            try
            {
                var data = await _context.Categories.FindAsync(request.Id);
                if(data == null)
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
                res.data = _mapper.Map<MRes_Category>(data);
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
                var data = await _context.Categories.FindAsync(id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.Categories.Remove(data);
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

        public async Task<ResponseData<MRes_Category>> GetById(int id)
        {
            var res = new ResponseData<MRes_Category>();
            try
            {
                var data = await _context.Categories.Include(x => x.Products).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var mapData = _mapper.Map<MRes_Category>(data);
                mapData.NumberOfProduct = data.Products.Count;
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

        public async Task<ResponseData<List<MRes_Category>>> GetList()
        {
            var res = new ResponseData<List<MRes_Category>>();
            try
            {
                var data = await _context.Categories.Include(x => x.Products).AsNoTracking().ToListAsync();
                res.data = _mapper.Map<List<MRes_Category>>(data);
                for(int i = 0; i < data.Count; i++)
                {
                    res.data[i].NumberOfProduct = data[i].Products.Count;
                }
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
