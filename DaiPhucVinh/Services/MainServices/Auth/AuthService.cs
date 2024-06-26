﻿using Falcon.Web.Core.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Shared.Constants;
using DaiPhucVinh.Shared.Account;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Services.Helper;
using System.Text;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace DaiPhucVinh.Services.MainServices.Auth
{
    public interface IAuthService
    {
        Task<TokenResponse> Login(TokenRequest request);
        Task<BaseResponse<AccountResponse>> LoginInFront(AccountRequest request);
        Task<BaseResponse<bool>> CheckStatusAccout(AccountRequest request);
        Task<Ticket> Validate(string userName, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly DataContext _context;
        private readonly ILogService _logService;

        public AuthService(IEncryptionService encryptionService, DataContext context, ILogService logService)
        {
            _encryptionService = encryptionService;
            _context = context;
            _logService = logService;
        }
        public async Task<TokenResponse> Login(TokenRequest request)
        {
            var result = new TokenResponse() { IsError = true };
            if (request == null)
                return result;
            var queryUserClaim = _context.UserClaims.AsQueryable();

            #region [Validate]
            var user = await Validate(request.UserName, request.Password);
            var role = await _context.Roles.FirstOrDefaultAsync(d => d.SystemName == user.RoleSystem);
            try
            {
                if (user != null && user.UserId > 0 && role != null)
                {
                    if (!user.IsActive)
                    {
                        result.ErrorDescription = GlobalMessage.LockUser;
                        return result;
                    }
                    // Login Web Admin
                    if (string.IsNullOrEmpty(request.PhoneOs))
                    {
                        if (user.RoleSystem.Equals("KyThuatVien"))
                        {
                            var employee = await _context.WMS_Employees.FirstOrDefaultAsync(d => d.UserLogin == request.UserName);
                            if (employee == null)
                            {
                                result.ErrorDescription = GlobalMessage.EmployeeUser;
                                return result;
                            }
                            if (string.IsNullOrEmpty(request.PhoneOs) && user.RoleSystem == null)
                            {
                                result.ErrorDescription = GlobalMessage.AccessDenied;
                                return result;
                            }
                            var userClaim = queryUserClaim.FirstOrDefault(s => s.ClaimName.Equals(user.RoleSystem));
                            if (userClaim != null)
                            {
                                if (string.IsNullOrEmpty(userClaim.ClaimValue))
                                {
                                    result.ErrorDescription = GlobalMessage.AccessDenied;
                                    return result;
                                }
                            }

                            result.ExpiresIn = 24;
                            result.ExpiredAt = DateTime.Now.AddHours(result.ExpiresIn);
                            result.AccessToken = TokenHelper.CreateToken(
                                result.ExpiresIn,
                                new Ticket
                                {
                                    UserId = user.UserId,
                                    Role = user.Role,
                                    RoleSystem = user.RoleSystem,
                                    Claims = user.Claims,
                                    UserName = request.UserName,
                                    LocationCode = employee?.LocationCode ?? ""
                                });
                            result.FullName = user.DisplayName;
                            result.UserName = request.UserName;
                            result.LocationCode = employee?.LocationCode ?? "";
                            result.Role = user.Role;
                            result.RoleSystem = user.RoleSystem;
                            result.RefreshToken = $"{user.UserId}.{Guid.NewGuid()}";
                            result.IdUser = user.UserId;
                            result.IsError = false;
                            result.TokenType = "bearer";
                            result.LockEditProduct = role.LockEditProduct;
                        }
                        else if (user.RoleSystem.Equals("KyThuatVien"))
                        {
                            var employee = await _context.WMS_Employees.FirstOrDefaultAsync(d => d.UserLogin == request.UserName);
                            if (employee == null)
                            {
                                result.ErrorDescription = GlobalMessage.EmployeeUser;
                                return result;
                            }
                            if (string.IsNullOrEmpty(request.PhoneOs) && user.RoleSystem == null)
                            {
                                result.ErrorDescription = GlobalMessage.AccessDenied;
                                return result;
                            }
                            var userClaim = queryUserClaim.FirstOrDefault(s => s.ClaimName.Equals(user.RoleSystem));
                            if (userClaim != null)
                            {
                                if (string.IsNullOrEmpty(userClaim.ClaimValue))
                                {
                                    result.ErrorDescription = GlobalMessage.AccessDenied;
                                    return result;
                                }
                            }

                            result.ExpiresIn = 24;
                            result.ExpiredAt = DateTime.Now.AddHours(result.ExpiresIn);
                            result.AccessToken = TokenHelper.CreateToken(
                                result.ExpiresIn,
                                new Ticket
                                {
                                    UserId = user.UserId,
                                    Role = user.Role,
                                    RoleSystem = user.RoleSystem,
                                    Claims = user.Claims,
                                    UserName = request.UserName,
                                    LocationCode = employee?.LocationCode ?? ""
                                });
                            result.FullName = user.DisplayName;
                            result.UserName = request.UserName;
                            result.LocationCode = employee?.LocationCode ?? "";
                            result.Role = user.Role;
                            result.RoleSystem = user.RoleSystem;
                            result.RefreshToken = $"{user.UserId}.{Guid.NewGuid()}";
                            result.IdUser = user.UserId;
                            result.IsError = false;
                            result.TokenType = "bearer"; 
                            result.LockEditProduct = role.LockEditProduct;
                        }
                        else
                        {
                            if (user.RoleSystem == null)
                            {
                                result.ErrorDescription = GlobalMessage.AccessDenied;
                                return result;
                            }
                            var employee = await _context.WMS_Employees.FirstOrDefaultAsync(d => d.UserLogin == request.UserName);
                            result.ExpiresIn = 24;
                            result.ExpiredAt = DateTime.Now.AddHours(result.ExpiresIn);
                            result.AccessToken = TokenHelper.CreateToken(
                                result.ExpiresIn,
                                new Ticket
                                {
                                    UserId = user.UserId,
                                    Role = user.Role,
                                    RoleSystem = user.RoleSystem,
                                    Claims = user.Claims,
                                    UserName = request.UserName,
                                    LocationCode = employee?.LocationCode ?? ""
                                });
                            result.FullName = user.DisplayName;
                            result.UserName = request.UserName;
                            result.LocationCode = employee?.LocationCode ?? "";
                            result.Role = user.Role;
                            result.RoleSystem = user.RoleSystem;
                            result.RefreshToken = $"{user.UserId}.{Guid.NewGuid()}";
                            result.IdUser = user.UserId;
                            result.IsError = false;
                            result.TokenType = "bearer";
                            result.LockEditProduct = role.LockEditProduct;
                        }
                    }
                    //Login App mobile
                    else
                    {
                        // Quyền khách hàng
                        var customer = await _context.WMS_Customers.FirstOrDefaultAsync(d => d.UserId == user.UserId);
                        if (user.RoleSystem.Equals("KhachHang"))
                        {
                            if (customer == null)
                            {
                                result.ErrorDescription = GlobalMessage.CustomerUser;
                                return result;
                            }

                            result.ExpiresIn = 24;
                            result.ExpiredAt = DateTime.Now.AddHours(result.ExpiresIn);
                            result.AccessToken = TokenHelper.CreateToken(
                                result.ExpiresIn,
                                new Ticket
                                {
                                    UserId = user.UserId,
                                    Role = user.Role,
                                    RoleSystem = user.RoleSystem,
                                    Claims = user.Claims,
                                    UserName = request.UserName,
                                    LocationCode = ""
                                });
                            result.FullName = user.DisplayName;
                            result.UserName = request.UserName;
                            result.LocationCode = "";
                            result.Role = user.Role;
                            result.RoleSystem = user.RoleSystem;
                            result.RefreshToken = $"{user.UserId}.{Guid.NewGuid()}";
                            result.IdUser = user.UserId;
                            result.IsError = false;
                            result.TokenType = "bearer";
                            result.CustomerCode = customer.Code;
                        }
                        // Quyền Kỹ thuật viên
                        else if (user.RoleSystem.Equals("KyThuatVien"))
                        {
                            var employee = await _context.WMS_Employees.FirstOrDefaultAsync(d => d.UserLogin == request.UserName);
                            if (employee == null)
                            {
                                result.ErrorDescription = GlobalMessage.EmployeeUser;
                                return result;
                            }
                            var location = await _context.WMS_Locations.FirstOrDefaultAsync(d => d.Code == employee.LocationCode);
                            result.ExpiresIn = 24;
                            result.ExpiredAt = DateTime.Now.AddHours(result.ExpiresIn);
                            result.AccessToken = TokenHelper.CreateToken(
                                result.ExpiresIn,
                                new Ticket
                                {
                                    UserId = user.UserId,
                                    Role = user.Role,
                                    RoleSystem = user.RoleSystem,
                                    Claims = user.Claims,
                                    UserName = request.UserName,
                                    LocationCode = employee?.LocationCode ?? ""
                                });
                            result.FullName = user.DisplayName;
                            result.UserName = request.UserName;
                            result.LocationCode = employee?.LocationCode ?? "";
                            result.Long = location?.Long ?? 0;
                            result.Lat = location?.Lat ?? 0;
                            result.Role = user.Role;
                            result.RoleSystem = user.RoleSystem;
                            result.RefreshToken = $"{user.UserId}.{Guid.NewGuid()}";
                            result.IdUser = user.UserId;
                            result.IsError = false;
                            result.TokenType = "bearer";
                        }
                        else
                        {
                            result.ErrorDescription = GlobalMessage.AccessDenied;
                            return result;
                        }
                    }
                }
                else
                {
                    result.ErrorDescription = GlobalMessage.InvalidUsernamePassword;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
            
            #endregion

            return result;
        }

        public async Task<BaseResponse<AccountResponse>> LoginInFront(AccountRequest request)
        {
            var result = new BaseResponse<AccountResponse> { };
            try
            {
                var checkAccount = await _context.EN_Account.FirstOrDefaultAsync(x => x.Username.Equals(request.Username));
                if(checkAccount == null)
                {
                    result.Message = "Email chưa đăng ký tài khoản trên hệ thống. Vui lòng liện hệ quán trị hệ thống để đăng ký tài khoản";
                    result.Success = false;
                }
                var query = _context.EN_Account.AsQueryable();
                string covertPass = MD5Hash(Base64Encode(request.Password));   
                query = query.Where(d => d.Username.Equals(request.Username) && d.Password.Equals(covertPass));
                var checkRole = await _context.EN_AccountType.FindAsync(checkAccount.AccountId);
                result.DataCount = await query.CountAsync();
                if (result.DataCount > 0)
                {
                    var data = await query.ToListAsync();
                    
                    if (data[0].Status == true)
                    {
                        var resultList = data.MapTo<AccountResponse>();
                        resultList[0].Name = checkRole.Name;
                        resultList[0].AccessToken = TokenHelper.CreateToken(
                                                   24,
                                                   new Ticket
                                                   {
                                                       UserId = resultList[0].UserId,
                                                       Role = resultList[0].Name,
                                                       RoleSystem = resultList[0].Name,
                                                       Claims = null,
                                                       UserName = request.Username,
                                                       LocationCode = ""
                                                   }); 
                        result.Data = resultList;
                        result.Message = "Đăng nhập thành công";
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Tài khoản của bạn đã bị khóa. Vui lòng liện hệ quản trị hệ thống để biết thêm chi tiết";
                    }

                }
                else
                {
                    result.Message = "Tài khoản hoặc mật khẩu không đúng";
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<Ticket> Validate(string userName, string password)
        {
            var result = new Ticket();
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(aa => aa.UserName == userName);
            
            #region [Validate password]
            if (user == null || user.Password != _encryptionService.CreatePasswordHash(password, user.Salt))
            {
                return result;
            }
            if (user != null && !user.Active)
            {
                result.UserId = user.Id;
                result.IsActive = false;
                return result;
            }
            #endregion
            result.UserId = user.Id;
            result.DisplayName = user.DisplayName;
            result.Claims = user.UserClaims.ToDictionary(x => x.ClaimName, x => x.ClaimValue);
            result.Role = user.Roles;
            result.RoleSystem = user.RoleSystem;
            result.IsActive = !(!user.Active || user.Deleted);
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public async Task<BaseResponse<bool>> CheckStatusAccout(AccountRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var checkAccount = await _context.EN_Account.FirstOrDefaultAsync(x => x.Username.Equals(request.Username));
                if(checkAccount == null)
                {
                    result.Success = false;
                    result.Message = "Email chưa đăng ký tài khoản trên hệ thống. Vui lòng liện hệ quán trị hệ thống để đăng ký tài khoản";
                    return result;
                }
                else
                {
                    if(checkAccount.Status == true)
                    {
                        result.Success = true;
                        result.Message = "Đăng nhập thành công";
                        return result;
                    }
                    result.Success = false;
                    result.Message = "Tài khoản của bạn đã bị khóa. Vui lòng liện hệ quản trị hệ thống để biết thêm chi tiết";
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
    }
}
