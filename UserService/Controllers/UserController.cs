using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Common;
using UserService.Models.Dtos.RequestModels;
using UserService.Models.Dtos.ResponseModels;
using UserService.Services;
using UserService.Ultils;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IS_User _s_User;

        public UserController(IS_User s_User)
        {
            _s_User = s_User;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(MReq_UserLogin request)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseData<MRes_User>(0, 400, DataAnnotationExtensionMethod.GetErrorMessage(ModelState)));
            var res = await _s_User.Login(request);
            return Ok(res);
        }

        [HttpPost("loginWithGoogle")]
        public async Task<IActionResult> LoginWithGG(MReq_UserLoginGoogle request)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseData<MRes_User>(0, 400, DataAnnotationExtensionMethod.GetErrorMessage(ModelState)));
            var res = await _s_User.LoginWithGoogle(request);
            return Ok(res);
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> Create(MReq_User request)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseData<MRes_User>(0, 400, DataAnnotationExtensionMethod.GetErrorMessage(ModelState)));
            var res = await _s_User.SignUp(request);
            return Ok(res); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _s_User.GetById(id);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _s_User.Delete(id);
            return Ok(res); 
        }

        [HttpPut("nameAvatarAddress")]
        public async Task<IActionResult> UpdateImageAndName(MReq_UserNameImageAddress request)
        {
            var res = await _s_User.UpdateImageNameAddress(request);
            return Ok(res);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(MReq_UserPassword request)
        {
            var res = await _s_User.UpdatePassword(request);
            return Ok(res);
        }

        [HttpPut("goPremium/{userId}")]
        public async Task<IActionResult> GoPremium(Guid userId)
        {
            var res = await _s_User.GoPremium(userId);
            return Ok(res);
        }

        [HttpPut("removePremium/{userId}")]
        public async Task<IActionResult> RemovePremium(Guid userId)
        {
            var res = await _s_User.RemovePremium(userId);
            return Ok(res);
        }

        [HttpPost("address")]
        public async Task<IActionResult> CreateAddress(MReq_UserAddress request)
        {
            var res = await _s_User.CreateAddress(request);
            return Ok(res);
        }

        [HttpGet("address/{userId}")]
        public async Task<IActionResult> GetUserAddresses(Guid userId)
        {
            var res = await _s_User.GetUserAddress(userId);
            return Ok(res);
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateUserAddress(MReq_UserAddress request)
        {
            var res = await _s_User.UpdateAddress(request);
            return Ok(res); 
        }

    }
}
