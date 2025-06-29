﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;
using System.Drawing;

namespace ProductService.Services
{
    public interface IS_ProductVariant
    {
        Task<ResponseData<MRes_ProductVariant>> Create(MReq_ProductVariant request);

        Task<ResponseData<MRes_ProductVariant>> Update(MReq_ProductVariant request);

        Task<ResponseData<int>> Delete(int id);

        Task<ResponseData<MRes_ProductVariant>> GetById(int id);

        Task<ResponseData<List<MRes_ProductVariant>>> GetList();

        Task<ResponseData<List<MRes_ProductVariant>>> GetListByProduct(int productId);
    }
    public class S_ProductVariant : IS_ProductVariant
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;

        public S_ProductVariant(ProductDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<MRes_ProductVariant>> Create(MReq_ProductVariant request)
        {
            var res = new ResponseData<MRes_ProductVariant>();
            try
            {
                var data = new ProductVariant();
                data.Id = request.Id;
                data.ProductId = request.ProductId;
                data.Size = request.Size;
                data.Color = request.Color;
                data.Price = request.Price;
                data.Status = request.Status;
                data.CreatedAt = DateTime.Now;

                _context.ProductVariants.Add(data);
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

        public async Task<ResponseData<MRes_ProductVariant>> Update(MReq_ProductVariant request)
        {
            var res = new ResponseData<MRes_ProductVariant>();
            try
            {
                var data = new ProductVariant();
                data.Id = request.Id;
                data.ProductId = request.ProductId;
                data.Size = request.Size;
                data.Color = request.Color;
                data.Price = request.Price;
                data.Status = request.Status;

                _context.ProductVariants.Update(data);
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
                var data = await _context.ProductVariants.FindAsync(id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.ProductVariants.Remove(data);

                var save = await _context.SaveChangesAsync();
                if (save == 0)
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

        public async Task<ResponseData<MRes_ProductVariant>> GetById(int id)
        {
            var res = new ResponseData<MRes_ProductVariant>();
            try
            {
                var data = await _context.ProductVariants.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_ProductVariant>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductVariant>>> GetList()
        {
            var res = new ResponseData<List<MRes_ProductVariant>>();
            try
            {
                var data = await _context.ProductVariants.AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_ProductVariant>>(data);
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductVariant>>> GetListByProduct(int productId)
        {
            var res = new ResponseData<List<MRes_ProductVariant>>();
            try
            {
                var data = await _context.ProductVariants.Where(x => x.ProductId == productId).AsNoTracking().ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_ProductVariant>>(data);
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
