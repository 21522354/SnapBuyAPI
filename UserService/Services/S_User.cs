﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UserService.Common;
using UserService.Models.Dtos.RequestModels;
using UserService.Models.Dtos.ResponseModels;
using UserService.Models.Entities;
using UserService.Ultils;

namespace UserService.Services
{
    public interface IS_User
    {
        Task<ResponseData<MRes_User>> GetById(Guid userId);
        Task<ResponseData<MRes_User>> SignUp(MReq_User request);
        Task<ResponseData<int>> Delete(Guid userId);
        Task<ResponseData<MRes_User>> UpdateImageAndName(MReq_UserNameImage request);
        Task<ResponseData<MRes_User>> UpdatePassword(MReq_UserPassword request);

    }
    public class S_User : IS_User
    {
        private readonly UserDBContext _context;
        private readonly IMapper _mapper;

        public S_User(UserDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<int>> Delete(Guid userId)
        {
            var res = new ResponseData<int>();
            try
            {
                var data = await _context.Users.FindAsync(userId);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                _context.Users.Remove(data);
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_DELETE;
                    return res;
                }
                res.data = save;
                res.result = 1;
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

        public async Task<ResponseData<MRes_User>> GetById(Guid userId)
        {
            var res = new ResponseData<MRes_User>();
            try
            {
                var data = await _context.Users.FindAsync(userId);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var mapData = _mapper.Map<MRes_User>(data);
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

        public async Task<ResponseData<MRes_User>> SignUp(MReq_User request)
        {
            var res = new ResponseData<MRes_User>();
            try
            {
                var existsUser = await _context.Users.FirstOrDefaultAsync(p => p.UserName == request.UserName || p.Email == request.Email) == null;
                if (existsUser)
                {
                    res.error.message = "Trùng email hoặc username";
                    return res;
                }
                var newUser = _mapper.Map<User>(request);
                newUser.ID = Guid.NewGuid();
                newUser.ImageURL = "https://cdn.kona-blue.com/upload/kona-blue_com/post/images/2024/09/18/457/avatar-mac-dinh-1.jpg";
                newUser.Name = "Default Name";
                _context.Users.Add(newUser);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }
                var addedUser = await GetById(newUser.ID);
                res.data = addedUser.data;
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

        public async Task<ResponseData<MRes_User>> UpdateImageAndName(MReq_UserNameImage request)
        {
            var res = new ResponseData<MRes_User>();
            try
            {
                var data = await _context.Users.FindAsync(request.ID);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.Name = request.Name;
                data.ImageURL = request.ImageUrl;
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    return res;
                }
                res.data = _mapper.Map<MRes_User>(data);
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

        public async Task<ResponseData<MRes_User>> UpdatePassword(MReq_UserPassword request)
        {
            var res = new ResponseData<MRes_User>();
            try
            {
                var data = await _context.Users.FindAsync(request.ID);
                if(data == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                data.Password = request.Password;
                var save = await _context.SaveChangesAsync();
                if(save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_UPDATE;
                    return res;
                }
                res.data = _mapper.Map<MRes_User>(data);
                res.result = 1;
            }
            catch(Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }
    }
}
