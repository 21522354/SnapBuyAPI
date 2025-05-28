using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProductService.Common;
using ProductService.Models.Dtos.RequestModels;
using ProductService.Models.Dtos.ResponseModels;
using ProductService.Models.Entities;
using ProductService.SyncDataService;
using ProductService.Ultils;

namespace ProductService.Services
{
    public interface IS_ProductReview
    {
        Task<ResponseData<MRes_ProductReview>> Create(MReq_ProductReview request);
        Task<ResponseData<MRes_ProductReview>> Update(MReq_ProductReview request);
        Task<ResponseData<int>> Delete(int id);
        Task<ResponseData<MRes_ProductReview>> GetById(int id);
        Task<ResponseData<List<MRes_ProductReview>>> GetListByProduct(int productId);
    }
    public class S_ProductReview : IS_ProductReview
    {
        private readonly ProductDBContext _context;
        private readonly IMapper _mapper;
        private readonly IS_UserDataClient _s_UserDataClient;

        public S_ProductReview(ProductDBContext context, IMapper mapper, IS_UserDataClient s_UserDataClient)
        {
            _context = context;
            _mapper = mapper;
            _s_UserDataClient = s_UserDataClient;
        }

        public async Task<ResponseData<MRes_ProductReview>> Create(MReq_ProductReview request)
        {
            var res = new ResponseData<MRes_ProductReview>();
            try
            {
                var data = new ProductReview();
                data.Id = await _context.ProductReviews.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync() + 1;
                data.ProductId = request.ProductId;
                data.ProductNote = request.ProductNote;
                data.OrderId = request.OrderId;
                data.StarNumber = request.StarNumber;
                data.ReviewComment = request.ReviewComment;
                data.UserId = request.UserId;
                _context.ProductReviews.Add(data);

                var productReviewImages = request.ProductReviewImages.Select(x => new ProductReviewImage
                {
                    ProductReviewId = data.Id,
                    Url = x
                }).ToList();
                _context.ProductReviewImages.AddRange(productReviewImages);

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

        public async Task<ResponseData<MRes_ProductReview>> Update(MReq_ProductReview request)
        {
            var res = new ResponseData<MRes_ProductReview>();
            try
            {
                var data = await _context.ProductReviews.FindAsync(request.Id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.ProductId = request.ProductId;
                data.ProductNote = request.ProductNote;
                data.OrderId = request.OrderId;
                data.StarNumber = request.StarNumber;
                data.ReviewComment = request.ReviewComment;
                data.UserId = request.UserId;
                _context.ProductReviews.Update(data);

                var oldImages = await _context.ProductReviewImages.Where(x => x.ProductReviewId == data.Id).ToListAsync();
                _context.ProductReviewImages.RemoveRange(oldImages);

                var productReviewImages = request.ProductReviewImages.Select(x => new ProductReviewImage
                {
                    ProductReviewId = data.Id,
                    Url = x
                }).ToList();
                _context.ProductReviewImages.AddRange(productReviewImages);

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
                var data = await _context.ProductReviews.FindAsync(id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var productReviewImages = await _context.ProductReviewImages.Where(x => x.ProductReviewId == id).ToListAsync();
                _context.ProductReviewImages.RemoveRange(productReviewImages);
                _context.ProductReviews.Remove(data);

                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_DELETE;
                    return res;
                }
                res.result = 1;
                res.data = save;
                return res;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_ProductReview>> GetById(int id)
        {
            var res = new ResponseData<MRes_ProductReview>();
            try
            {
                var data = await _context.ProductReviews.Include(x => x.ProductReviewImages).FirstOrDefaultAsync(x => x.Id == id);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                res.result = 1;
                res.data = _mapper.Map<MRes_ProductReview>(data);
                res.data.ProductReviewImages = data.ProductReviewImages.Select(x => x.Url).ToList();
                var user = await _s_UserDataClient.GetUserById(res.data.UserId);
                res.data.User = user;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ProductReview>>> GetListByProduct(int productId)
        {
            var res = new ResponseData<List<MRes_ProductReview>>();
            try
            {
                var data = await _context.ProductReviews
                    .Include(x => x.ProductReviewImages)
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();
                res.result = 1;
                res.data = _mapper.Map<List<MRes_ProductReview>>(data);
                for(int i = 0; i < data.Count; i++)
                {
                    res.data[i].ProductReviewImages = data[i].ProductReviewImages.Select(x => x.Url).ToList();
                    res.data[i].User = await _s_UserDataClient.GetUserById(data[i].UserId);
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
    }
}
