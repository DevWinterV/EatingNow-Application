
using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Customer;
using DaiPhucVinh.Shared.User;
using Falcon.Web.Core.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.User
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponse>> TakeAllUsers(UserRequest request);
        Task<BaseResponse<UserResponse>> TakeAllUserForTask(UserRequest request);
        Task<BaseResponse<bool>> CreateUser(UserRequest request);
        Task<BaseResponse<bool>> UpdateUser(UserRequest request);
        Task<BaseResponse<bool>> ResetPassWord(UserRequest request);
        Task<BaseResponse<bool>> LockUser(UserRequest request);
        Task<BaseResponse<bool>> UnLockUser(UserRequest request);
        Task<BaseResponse<UserResponse>> TakeUserById(int Id);
        Task<BaseResponse<bool>> RemoveUser(UserRequest request);
        Task<BaseResponse<bool>> UnifyUser();
        Task<BaseResponse<bool>> ChangePassword(UserRequest request);
        Task<BaseResponse<UserResponse>> TakeUserByUserName(string userName);
        Task<BaseResponse<bool>> Signup(UserRequest request);
        Task<BaseResponse<bool>> CreateUserCustomer(UserRequest request);
    }
    public class UserService : IUserService
    {
        private readonly DataContext _datacontext;
        private readonly FesContext _fescontext;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private const int SaltLength = 12;
        public UserService(DataContext datacontext, IEncryptionService encryptionService, FesContext fescontext, ILogService logService, ICommonService commonService)
        {
            _datacontext = datacontext;
            _fescontext = fescontext;
            _encryptionService = encryptionService;
            _logService = logService;
            _commonService = commonService;
        }
        public async Task<BaseResponse<UserResponse>> TakeAllUsers(UserRequest request)
        {
            var result = new BaseResponse<UserResponse> { };
            try
            {
                //var query = _datacontext.Users.AsQueryable().Where(d => !d.Deleted && d.Active == request.Active);
                var query = _datacontext.Users.AsQueryable().Where(d => !d.Deleted && !d.Roles.Equals("Admin"));
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.UserName.Contains(request.Term) ||
                                             d.DisplayName.Contains(request.Term));
                }
                if (request.RoleSystem.HasValue() && !request.RoleSystem.Equals("All"))
                {
                    query = query.Where(d => d.RoleSystem.Equals(request.RoleSystem));
                }
                query = query.Where(d => d.Active == request.Active);
                
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<UserResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<UserResponse>> TakeAllUserForTask(UserRequest request)
        {
            var result = new BaseResponse<UserResponse> { };
            try
            {
                var query = _datacontext.Users.AsQueryable().Where(d => !d.Deleted && d.Active == request.Active && d.Roles == "Employee");
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.UserName.Contains(request.Term) ||
                                             d.DisplayName.Contains(request.Term));
                }

                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<UserResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<UserResponse>> TakeUserByUserName(string userName)
        {
            var result = new BaseResponse<UserResponse> { };
            try
            {
                var data = await _datacontext.Users.FirstOrDefaultAsync(d => d.UserName.ToLower() == userName.ToLower());
                result.Item = data.MapTo<UserResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<UserResponse>> TakeUserById(int Id)
        {
            var result = new BaseResponse<UserResponse> { };
            try
            {
                var data = await _datacontext.Users.FindAsync(Id);
                result.Item = data.MapTo<UserResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateUser(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if(request.Password == request.NewPassword)
                {
                    int Id = MaxIdEmployee();
                    var salt = _encryptionService.CreateSaltKey(SaltLength);
                    if (request.Id == 0)
                    {
                        var oldUser = await _datacontext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower() && !x.Deleted && x.Active);
                        if (oldUser == null)
                        {
                            var user = _datacontext.Users.Add(new Server.Data.Entity.User
                            {
                                UserName = request.UserName.ToLower(),
                                DisplayName = request.DisplayName,
                                Password = _encryptionService.CreatePasswordHash(request.Password, salt),
                                Salt = salt,
                                Active = true,
                                Roles = "Employee",
                                RoleSystem = request.RoleSystem,
                                Deleted = false,
                            });
                            var Permissons = _datacontext.Roles.FirstOrDefault(x => x.SystemName.Equals(request.RoleSystem));
                            var userClaim = _datacontext.UserClaims.Add(new UserClaim
                            {
                                UserId = user.Id,
                                ClaimName = Permissons.SystemName,
                                ClaimValue = Permissons.Permissons,
                                Deleted = false,
                            });
                            var employee = _datacontext.WMS_Employees.FirstOrDefault(x => x.UserLogin == user.UserName);
                            if(employee == null)
                            {
                                var UserEmployee = _datacontext.WMS_Employees.Add(new WMS_Employee
                                {
                                    Id = Id,
                                    EmployeeCode = await _commonService.AutoGencode(nameof(WMS_Employee)),
                                    UserLogin = user.UserName,
                                    FullName = user.DisplayName,
                                });
                            }
                        }
                        else
                        {
                            result.Message = "Tên đăng nhập đã tồn tại !";
                            result.Success = false;
                            return result;
                        }
                    }
                }
                else
                {
                    result.Message = "Nhập lại mật khẩu không trùng khớp !";
                    result.Success = false;
                    return result;
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateUser(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    var user = await _datacontext.Users.FindAsync(request.Id);
                    var canChangeUsername = await _datacontext.Users.FirstOrDefaultAsync(d => d.Id != request.Id && d.UserName.ToLower() == request.UserName.ToLower() && !d.Deleted && d.Active);
                    if (user != null && canChangeUsername == null)
                    {
                        user.UserName = request.UserName.ToLower();
                        user.DisplayName = request.DisplayName;
                        user.RoleSystem = request.RoleSystem;
                        if (!string.IsNullOrEmpty(request.RoleSystem))
                        {
                            var Permissons = _datacontext.Roles.FirstOrDefault(x => x.SystemName.Equals(request.RoleSystem));
                            var userClaim = _datacontext.UserClaims.SingleOrDefault(x => x.UserId == user.Id);
                            if (userClaim == null)
                            {
                                var user_Claim = _datacontext.UserClaims.Add(new UserClaim
                                {
                                    UserId = user.Id,
                                    ClaimName = user.RoleSystem != null ? user.RoleSystem : "",
                                    ClaimValue = Permissons != null ? Permissons.Permissons : "",
                                    Deleted = false,
                                });
                            }
                            else
                            {
                                userClaim.ClaimName = user.RoleSystem != null ? user.RoleSystem : "";
                                userClaim.ClaimValue = Permissons.Permissons != null ? Permissons.Permissons : "";
                            }
                        }
                        var employee = _datacontext.WMS_Employees.FirstOrDefault(x => x.UserLogin.Equals(user.UserName));
                        if (employee == null)
                        {
                            var UserEmployee = _datacontext.WMS_Employees.Add(new WMS_Employee
                            {
                                Id = MaxIdEmployee(),
                                EmployeeCode = await _commonService.AutoGencode(nameof(WMS_Employee)),
                                UserLogin = user.UserName,
                                FullName = user.DisplayName,
                            });
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Tên đăng nhập đã tồn tại !";
                        return result;
                    }
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        private int MaxIdEmployee()
        {
            var data = _datacontext.WMS_Employees.AsQueryable().ToArray();
            int max = data[0].Id;
            for (int i = 0; i < data.Length; i++)
            {
                if (max < data[i].Id)
                {
                    max = data[i].Id + 1;
                }
            }
            return max;
        }
        public async Task<BaseResponse<bool>> ResetPassWord(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Password == request.NewPassword)
                {
                    var salt = _encryptionService.CreateSaltKey(SaltLength);
                    if (request.Id > 0)
                    {
                        var user = await _datacontext.Users.FindAsync(request.Id);
                        if (user != null)
                        {
                            user.Password = _encryptionService.CreatePasswordHash(request.Password, salt);
                            user.Salt = salt;
                        }
                    }
                }
                else
                {
                    result.Message = "Nhập lại mật khẩu không trùng khớp !";
                    result.Success = false;
                    return result;
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> LockUser(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var user = await _datacontext.Users.FindAsync(request.Id);
                if(user != null)
                {
                    user.Active = false;
                    var userClaim = await _datacontext.UserClaims.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    if(userClaim != null)
                    {
                        userClaim.Deleted = true;
                    }
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UnLockUser(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var user = await _datacontext.Users.FindAsync(request.Id);
                if (user != null)
                {
                    user.Active = true;
                    var userClaim = await _datacontext.UserClaims.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    if (userClaim != null)
                    {
                        userClaim.Deleted = false;
                    }
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> RemoveUser(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var userClaim = await _datacontext.UserClaims.FirstOrDefaultAsync(x => x.UserId == request.Id);
                if (userClaim != null)
                {
                    _datacontext.UserClaims.Remove(userClaim);
                    var user = await _datacontext.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (user != null)
                    {
                        _datacontext.Users.Remove(user);
                    }
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UnifyUser()
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var testPass = "123456";
                var testRole = "Employee";

                var z_user = await _datacontext.Users.AsQueryable().Select(d => d.UserName).ToListAsync();
                var list_user = await _fescontext.ACL_User.AsQueryable().Where(x => !x.Disabled && !z_user.Contains(x.UserName)).ToListAsync();
                foreach (var new_user in list_user)
                {
                    var salt = _encryptionService.CreateSaltKey(SaltLength);
                    var password = _encryptionService.CreatePasswordHash(testPass, salt);
                    _datacontext.Users.Add(new Server.Data.Entity.User
                    {
                        UserName = new_user.UserName,
                        DisplayName = new_user.DisplayName,
                        Password = password,
                        Salt = salt,
                        Active = true,
                        Roles = testRole,
                        Deleted = false,
                    });
                }
                await _datacontext.SaveChangesAsync();
                result.DataCount = list_user.Count();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ChangePassword(UserRequest request)
        {
            var result = new BaseResponse<bool> { Success = false };
            try
            {
                var USER_ID = TokenHelper.CurrentIdentity().UserId;
                var user = await _datacontext.Users.FindAsync(USER_ID);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "Không tồn tại thông tin tài khoản.";
                    return result;
                }
                var pass = _encryptionService.CreatePasswordHash(request.Password, user.Salt);
                if (user.Password != pass)
                {
                    result.Success = false;
                    result.Message = "Mật khẩu không đúng, xin vui lòng thử lại !";
                    return result;
                }
                var salt = _encryptionService.CreateSaltKey(SaltLength);
                user.Salt = salt;
                user.Password = _encryptionService.CreatePasswordHash(request.NewPassword, salt);
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateUserCustomer(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Password == request.NewPassword)
                {
                    var salt = _encryptionService.CreateSaltKey(SaltLength);
                    if (request.Id == 0)
                    {
                        var oldUser = await _datacontext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower() && !x.Deleted && x.Active);
                        if (oldUser == null)
                        {
                            var user = _datacontext.Users.Add(new Server.Data.Entity.User
                            {
                                UserName = request.UserName.ToLower(),
                                DisplayName = request.DisplayName,
                                Password = _encryptionService.CreatePasswordHash(request.Password, salt),
                                Salt = salt,
                                Active = true,
                                Roles = "Customer",
                                RoleSystem = "KhachHang",
                                Deleted = false,
                            });
                            var Permissons = _datacontext.Roles.FirstOrDefault(x => x.SystemName == "KhachHang");
                            var userClaim = _datacontext.UserClaims.Add(new UserClaim
                            {
                                UserId = user.Id,
                                ClaimName = Permissons.SystemName,
                                ClaimValue = Permissons.Permissons,
                                Deleted = false,
                            });
                            await _datacontext.SaveChangesAsync();
                            var customer = _datacontext.WMS_Customers.FirstOrDefault(x => x.Code.Equals(request.CustomerCode));
                            if (customer != null)
                            {
                                customer.UserId = user.Id;
                                customer.LastUpdateDate = DateTime.Now;
                                customer.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                            }
                        }
                        else
                        {
                            result.Message = "Tên đăng nhập đã tồn tại !";
                            result.Success = false;
                            return result;
                        }
                    }
                }
                else
                {
                    result.Message = "Nhập lại mật khẩu không trùng khớp !";
                    result.Success = false;
                    return result;
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        #region [App]
        public async Task<BaseResponse<bool>> Signup(UserRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var salt = _encryptionService.CreateSaltKey(SaltLength);

                var User = await _datacontext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());
                var queryCustomer = _datacontext.WMS_Customers.AsQueryable();
                if (User == null)
                {
                    var user = _datacontext.Users.Add(new Server.Data.Entity.User
                    {
                        UserName = request.UserName.ToLower(),
                        DisplayName = request.CustomerName,
                        Password = _encryptionService.CreatePasswordHash(request.Password, salt),
                        Salt = salt,
                        Active = true,
                        Roles = "Customer",
                        Deleted = false,
                    });

                    var cus = _datacontext.WMS_Customers.Add(new WMS_Customers
                    {
                        Code = await _commonService.AutoGencode(nameof(WMS_Customers)),
                        Name = request.CustomerName,
                        Name_Unsigned = request.CustomerName.ConvertToUnSign(),
                        Address = request.Address,
                        PhoneNo = request.PhoneNo,
                        TaxCode = request.MaSoThue,
                        PersonContact = request.NguoiDaiDien,
                        CustomerType_Id = 3,
                        CreationDate = DateTime.Now,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                        UserId = user.Id
                    });
                    _datacontext.SaveChanges();
                    result.Success = true;
                }
                else
                {
                    result.Message = "Tên đăng nhập đã tồn tại";
                    result.Success = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        #endregion
    }
}