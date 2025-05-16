using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.Ultils;
using System.Linq.Expressions;

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

        Task<ResponseData<List<MRes_Product>>> GetListByCategoryId(int categoryId);

        Task<ResponseData<List<MRes_ProductRecommend>>> GetListProductStringForRecommend();
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
                var existCategory = await _context.Categories.AnyAsync(x => x.Id == request.CategoryId);
                if(!existCategory)
                {
                    res.error.message = "Không tồn tại category được chọn trong hệ thống";
                    return res;
                }

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

                _context.Products.Update(data);
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

        public async Task<ResponseData<MRes_Product>> GetById(int id)
        {
            var res = new ResponseData<MRes_Product>();
            try
            {
                var data = await _context.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.ProductVariants)
                    .Include(x => x.ProductTags)
                    .ThenInclude(x => x.Tag)
                    .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }

                var returnData = new MRes_Product();
                returnData.Id = data.Id;
                returnData.SellerId = data.SellerId;
                returnData.Name = data.Name;
                returnData.Description = data.Description;
                returnData.BasePrice = data.BasePrice;
                returnData.Status = data.Status;
                returnData.CategoryId = data.CategoryId;
                returnData.Quantity = data.Quantity;
                returnData.CreatedAt = data.CreatedAt;
                returnData.UpdatedAt = data.UpdatedAt;
                returnData.ProductImages = _mapper.Map<List<MRes_ProductImage>>(data.ProductImages);
                returnData.ProductVariants = _mapper.Map<List<MRes_ProductVariant>>(data.ProductVariants);
                returnData.ListTag = data.ProductTags.Select(x => x.Tag.TagName).ToList();

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

        public async Task<ResponseData<List<MRes_Product>>> GetList()
        {
            var res = new ResponseData<List<MRes_Product>>();
            try
            {
                var data = await _context.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.ProductVariants)
                    .Include(x => x.ProductTags)
                    .ThenInclude(x => x.Tag)
                    .AsNoTracking().ToListAsync();

                var returnData = data.Select(x => new MRes_Product
                {
                    Id = x.Id,
                    SellerId = x.SellerId,
                    Name = x.Name,
                    Description = x.Description,
                    BasePrice = x.BasePrice,
                    Status = x.Status,
                    CategoryId = x.CategoryId,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ProductImages = _mapper.Map<List<MRes_ProductImage>>(x.ProductImages),
                    ProductVariants = _mapper.Map<List<MRes_ProductVariant>>(x.ProductVariants),
                    ListTag = x.ProductTags.Select(x => x.Tag.TagName).ToList(),
                }).ToList();

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

        public async Task<ResponseData<List<MRes_Product>>> GetListBySellerId(Guid sellerId)
        {
            var res = new ResponseData<List<MRes_Product>>();
            try
            {
                var data = await _context.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.ProductVariants)
                    .Include(x => x.ProductTags)
                    .ThenInclude(x => x.Tag)
                    .Where(x => x.SellerId == sellerId)
                    .AsNoTracking().ToListAsync();

                var returnData = data.Select(x => new MRes_Product
                {
                    Id = x.Id,
                    SellerId = x.SellerId,
                    Name = x.Name,
                    Description = x.Description,
                    BasePrice = x.BasePrice,
                    Status = x.Status,
                    CategoryId = x.CategoryId,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ProductImages = _mapper.Map<List<MRes_ProductImage>>(x.ProductImages),
                    ProductVariants = _mapper.Map<List<MRes_ProductVariant>>(x.ProductVariants),
                    ListTag = x.ProductTags.Select(x => x.Tag.TagName).ToList(),
                }).ToList();

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

        public async Task<ResponseData<List<MRes_Product>>> GetListByCategoryId(int categoryId)
        {
            var res = new ResponseData<List<MRes_Product>>();
            try
            {
                var data = await _context.Products
                    .Include(x => x.ProductImages)
                    .Include(x => x.ProductVariants)
                    .Include(x => x.ProductTags)
                    .ThenInclude(x => x.Tag)
                    .Where(x => x.CategoryId == categoryId)
                    .AsNoTracking().ToListAsync();

                var returnData = data.Select(x => new MRes_Product
                {
                    Id = x.Id,
                    SellerId = x.SellerId,
                    Name = x.Name,
                    Description = x.Description,
                    BasePrice = x.BasePrice,
                    Status = x.Status,
                    CategoryId = x.CategoryId,
                    Quantity = x.Quantity,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ProductImages = _mapper.Map<List<MRes_ProductImage>>(x.ProductImages),
                    ProductVariants = _mapper.Map<List<MRes_ProductVariant>>(x.ProductVariants),
                    ListTag = x.ProductTags.Select(x => x.Tag.TagName).ToList(),
                }).ToList();

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

        public async Task<ResponseData<List<MRes_ProductRecommend>>> GetListProductStringForRecommend()
        {
            var res = new ResponseData<List<MRes_ProductRecommend>>();
            try
            {
                var products = await _context.Products.Include(x => x.ProductTags).ThenInclude(x => x.Tag).AsNoTracking().ToListAsync();
                var returnData = new List<MRes_ProductRecommend>();
                foreach (var product in products)
                {
                    var tempString = "";
                    tempString += product.Name;
                    tempString += "_";
                    tempString += product.BasePrice;
                    tempString += "_";

                    var productTags = product.ProductTags.Select(x => x.Tag.TagName).ToList();
                    for (int i = 0; i < productTags.Count; i++)
                    {
                        tempString += productTags[i];
                        if (i != productTags.Count - 1)
                        {
                            tempString += "_";
                        }
                    }

                    var data = new MRes_ProductRecommend
                    {
                        Id = product.Id,
                        ProductString = tempString
                    };

                    returnData.Add(data);
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
    }
}
