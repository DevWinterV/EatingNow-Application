using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Auth;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.User;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;

namespace DaiPhucVinh.ApiApp
{
    [RoutePrefix("xapi/oauth")]
    public class OAuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        public OAuthController(IAuthService authService, IUserService userService, ICommonService commonService)
        {
            _authService = authService;
            _userService = userService;
            _commonService = commonService;
        }
        [HttpPost]
        [Route("token")]
        public async Task<TokenResponse> Login(TokenRequest request) => await _authService.Login(request);

        [HttpPost]
        [Route("Signup")]
        public async Task<BaseResponse<bool>> Signup(UserRequest request) => await _userService.Signup(request);
        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<BaseResponse<bool>> ChangePassword([FromBody] UserRequest request) => await _userService.ChangePassword(request);
        [HttpPost]
        [Route("SaveDeviceToken")]
        public async Task<BaseResponse<MobileDeviceDto>> SaveDeviceToken([FromBody] MobileTokenRequest request)
        {
            return await _commonService.SaveDeviceToken(request);
        }
    }
}