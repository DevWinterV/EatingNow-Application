using DaiPhucVinh.Api;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.MainServices.Auth;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/role")]
    public class RoleController : SecureApiController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost]
        [Route("TakeListRoles")]
        public async Task<BaseResponse<RolesResponse>> TakeListRoles([FromBody] RolesRequest request) => await _roleService.TakeListRoles(request);
        [HttpPost]
        [Route("CreateRole")]
        public async Task<BaseResponse<bool>> CreateRole([FromBody] RoleRequest request) => await _roleService.CreateRole(request);
        [Route("UpdateRole")]
        public async Task<BaseResponse<bool>> UpdateRole([FromBody] RoleRequest request) => await _roleService.UpdateRole(request);
        [HttpPost]
        [Route("DeleteRole")]
        public async Task<BaseResponse<bool>> DeleteRole([FromBody] RoleRequest request) => await _roleService.DeleteRole(request);

        [HttpPost]
        [Authorize]
        [Route("GetPermissons")]
        public async Task<BaseResponse<string>> GetPermisson()
        {
            var token = TokenHelper.CurrentIdentity();
            var request = new UserLocalRequest()
            {
                UserId = token.UserId,
                RoleName = token.RoleSystem,
            };
            return await _roleService.lstPermisson(request);
        }
        [HttpPost]
        [Route("ChangeLockEditProduct")]
        public async Task<BaseResponse<bool>> ChangeLockEditProduct([FromBody] RoleRequest request) => await _roleService.ChangeLockEditProduct(request);

    }
}
