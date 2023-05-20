using DaiPhucVinh.Api;
using DaiPhucVinh.Shared.Common;
using System.Threading.Tasks;
using System.Web.Http;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Shared.User;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/user")]
    public class UserController : SecureApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("TakeAllUsers")]
        public async Task<BaseResponse<UserResponse>> TakeAllUsers([FromBody] UserRequest request) => await _userService.TakeAllUsers(request);
        [HttpPost]
        [Route("TakeAllUserForTask")]
        public async Task<BaseResponse<UserResponse>> TakeAllUserForTask([FromBody] UserRequest request) => await _userService.TakeAllUserForTask(request);
        [HttpGet]
        [Route("TakeUserById")]
        public async Task<BaseResponse<UserResponse>> TakeUserById(int Id) => await _userService.TakeUserById(Id);
        [HttpPost]
        [Route("CreateUser")]
        public async Task<BaseResponse<bool>> CreateUser([FromBody] UserRequest request) => await _userService.CreateUser(request);
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<BaseResponse<bool>> UpdateUser([FromBody] UserRequest request) => await _userService.UpdateUser(request);
        [HttpPost]
        [Route("ResetPassWord")]
        public async Task<BaseResponse<bool>> ResetPassWord([FromBody] UserRequest request) => await _userService.ResetPassWord(request);
        [HttpPost]
        [Route("LockUser")]
        public async Task<BaseResponse<bool>> LockUser([FromBody] UserRequest request) => await _userService.LockUser(request);
        [HttpPost]
        [Route("UnLockUser")]
        public async Task<BaseResponse<bool>> UnLockUser([FromBody] UserRequest request) => await _userService.UnLockUser(request);
        [HttpPost]
        [Route("RemoveUser")]
        public async Task<BaseResponse<bool>> RemoveUser([FromBody] UserRequest request) => await _userService.RemoveUser(request);
        [HttpPost]
        [Route("UnifyUser")]
        public async Task<BaseResponse<bool>> UnifyUser() => await _userService.UnifyUser();
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<BaseResponse<bool>> ChangePassword([FromBody] UserRequest request) => await _userService.ChangePassword(request);
        [HttpPost]
        [Route("CreateUserCustomer")]
        public async Task<BaseResponse<bool>> CreateUserCustomer([FromBody] UserRequest request) => await _userService.CreateUserCustomer(request);
    }
}



