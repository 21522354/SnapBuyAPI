using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;

namespace ProductService.Services
{
    public interface IS_Tag
    {
        Task<ResponseData<MRes_Tag>> Create(MReq_Tag request);

        Task<ResponseData<MRes_Tag>> Update(MReq_Tag request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_Tag>> GetById(int id);

        Task<ResponseData<List<MRes_Tag>>> GetList();
    }
    public class S_Tag : IS_Tag
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_Tag(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_Tag>> Create(MReq_Tag request)
        {
            var res = new ResponseData<MRes_Tag>();
            try
            {
                var data = new Tag();
                _mapper.Map(request, data);
                data.CreatedAt = DateTime.Now;
                _context.Tags.Add(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                }
                var getById = await GetById(data.Id);
                res.result = 1;
                res.data = _mapper.Map<MRes_Tag>(data);
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
        public async Task<ResponseData<MRes_Tag>> Update(MReq_Tag request)
        {
            var res = new ResponseData<MRes_Tag>();
            try
            {
                var data = await _context.Tags.FindAsync(request.Id);
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
                res.data = _mapper.Map<MRes_Tag>(data);
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
                var data = await _context.Tags.FindAsync(id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.Tags.Remove(data);
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

        public async Task<ResponseData<MRes_Tag>> GetById(int id)
        {
            var res = new ResponseData<MRes_Tag>();
            try
            {
                var data = await _context.Tags.Include(x => x.ProductTags).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var mapData = _mapper.Map<MRes_Tag>(data);
                mapData.NumberOfProduct = data.ProductTags.Count;
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

        public async Task<ResponseData<List<MRes_Tag>>> GetList()
        {
            var res = new ResponseData<List<MRes_Tag>>();
            try
            {
                var data = await _context.Tags.Include(x => x.ProductTags).AsNoTracking().ToListAsync();
                res.data = _mapper.Map<List<MRes_Tag>>(data);
                for (int i = 0; i < data.Count; i++)
                {
                    res.data[i].NumberOfProduct = data[i].ProductTags.Count;
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
