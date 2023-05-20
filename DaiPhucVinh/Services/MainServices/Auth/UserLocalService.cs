using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Shared.Common;
using Falcon.Web.Core.Auth;
using Falcon.Web.Core.Caching;
using Falcon.Web.Core.Security;
using Newtonsoft.Json;
using Pos123.Shared.Auth;

namespace DaiPhucVinh.Services.MainServices.Auth
{
    public interface IUserLocalService
    {
        Task<BaseResponse<UserClaimDto>> SearchRoleByUserId(UserLocalRequest request);
        Task<BaseResponse<UserDto>> TakeUserByRole(UserLocalRequest request);
    }
    public class UserLocalService: IUserLocalService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IEncryptionService _encryptionService;
        private readonly DataContext _datacontext;
        private readonly FesContext _fescontext;
        private const int SaltLenght = 6;
        public UserLocalService(ICacheManager cacheManager, IEncryptionService encryptionService, DataContext datacontext, FesContext fescontext)
        {
            _cacheManager = cacheManager;
            _encryptionService = encryptionService;
            _datacontext = datacontext;
            _fescontext = fescontext;
        }
        public async Task<BaseResponse<UserClaimDto>> SearchRoleByUserId(UserLocalRequest request)
        {
            var result = new BaseResponse<UserClaimDto> { Success = false };
            try
            {
                if (!string.IsNullOrEmpty(request.UserLogin))
                {
                    var query = _datacontext.UserClaims.Where(cc => cc.User.UserName.Equals(request.UserLogin) && cc.Deleted.HasValue && !cc.Deleted.Value);
                    var data = await query.OrderBy(cc => cc.ClaimName).ToListAsync();
                    result.Data = data.MapTo<UserClaimDto>();
                }                  
                result.Success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#else
                result.Message = ex.Message;
#endif
            }
            return result;
        }

        public async Task<BaseResponse<UserDto>> TakeUserByRole(UserLocalRequest request)
        {
            var result = new BaseResponse<UserDto> { Success = false };
            try
            {
                var data = await _datacontext.Users.FirstOrDefaultAsync(x => x.Active && x.Roles == request.RoleName 
                && x.UserName.ToLower() == request.UserLogin.ToLower());

                result.CustomData = data.MapTo<UserDto>();
                result.Success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#endif
            }
            return result;
        }
    }
}
