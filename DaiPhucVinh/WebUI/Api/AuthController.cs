using DaiPhucVinh.Services.MainServices.Auth;
using DaiPhucVinh.Shared.Account;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Shared.Common;
using System.Threading.Tasks;
using System.Web.Http;

namespace DaiPhucVinh.Api
{
    [RoutePrefix("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("token")]
        public async Task<TokenResponse> Login(TokenRequest request) => await _authService.Login(request);
        
        [HttpPost]
        [Route("LoginInFront")]
        public async Task<BaseResponse<AccountResponse>> LoginInFront(AccountRequest request) => await _authService.LoginInFront(request);
    }
}