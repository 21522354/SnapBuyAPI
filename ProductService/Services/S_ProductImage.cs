using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;
using System;

namespace ProductService.Services
{
    public interface IS_ProductImage
    {
        Task<ResponseData<MRes_ProductImage>> Create(MReq_ProductImage request);

        Task<ResponseData<MRes_ProductImage>> Update(MReq_ProductImage request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_ProductImage>> GetById(int id);

        Task<ResponseData<List<MRes_ProductImage>>> GetList();

        Task<ResponseData<List<MRes_ProductImage>>> GetListByProduct(int productId);
    }
    public class S_ProductImage : IS_ProductImage
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_ProductImage(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_ProductImage>> Create(MReq_ProductImage request)
        {
            var res = new ResponseData<MRes_ProductImage>();
            try
            {
                var data = new ProductImage();
                data.Id = request.Id;
                data.Url = request.Url;
                data.IsThumbnail = request.IsThumbnail;
                data.ProductId = request.ProductId;
                data.CreatedAt = DateTime.Now;


                _context.ProductImages.Add(data);
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

        public async Task<ResponseData<MRes_ProductImage>> Update(MReq_ProductImage request)
        {
            var res = new ResponseData<MRes_ProductImage>();
            try
            {
                var data = await _context.ProductImages.FindAsync(request.Id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.Id = request.Id;
                data.Url = request.Url;
                data.IsThumbnail = request.IsThumbnail;
                data.ProductId = request.ProductId;
                data.CreatedAt = DateTime.Now;


                _context.ProductImages.Update(data);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
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
                var data = await _context.ProductImages.FindAsync(id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.ProductImages.Remove(data);

                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_DELETE;
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

        public async Task<ResponseData<MRes_ProductImage>> GetById(int id)
        {
            var res = new ResponseData<MRes_ProductImage>();
            try
            {
                var data = await _context.ProductImages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_ProductImage>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductImage>>> GetList()
        {
            var res = new ResponseData<List<MRes_ProductImage>>();
            try
            {
                var data = await _context.ProductImages.AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_ProductImage>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductImage>>> GetListByProduct(int productId)
        {
            var res = new ResponseData<List<MRes_ProductImage>>();
            try
            {
                var data = await _context.ProductImages.Where(x => x.ProductId == productId).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_ProductImage>>(data);
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
