using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.User;
using Falcon.Web.Core.Caching;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Server.Data.DaiPhucVinh;

namespace DaiPhucVinh.Services.MainServices.Auth
{
    public interface IRoleService
    {
        Task<BaseResponse<RolesResponse>> TakeListRoles(RolesRequest request);
        Task<BaseResponse<bool>> CreateRole(RoleRequest request);
        Task<BaseResponse<bool>> UpdateRole(RoleRequest request);
        Task<BaseResponse<bool>> DeleteRole(RoleRequest request);

        Task<BaseResponse<string>> lstPermisson(UserLocalRequest request);
        Task<BaseResponse<RoleDto>> UpdateRole(UserLocalRequest request);
        Task<BaseResponse<bool>> ChangeLockEditProduct(RoleRequest request);
    }

    public class RoleService : IRoleService
    {
        private readonly DataContext _datacontext;
        private readonly FesContext _fescontext;
        private readonly ICacheManager _cacheManager;
        private readonly ILogService _logService;
        public RoleService(ICacheManager cacheManager, ILogService logService, DataContext datacontext, FesContext fescontext)
        {
            _cacheManager = cacheManager;
            _logService = logService;
            _datacontext = datacontext;
            _fescontext = fescontext;
        }
        public async Task<BaseResponse<RolesResponse>> TakeListRoles(RolesRequest request)
        {
            var result = new BaseResponse<RolesResponse> { };
            try
            {
                var query = _datacontext.Roles.AsQueryable().Where(d => !d.UserCanDeleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.SystemName.Contains(request.Term) ||
                                             d.Display.Contains(request.Term));
                }
                query = query.Where(d => d.Active == request.Active);
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.SystemName).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<RolesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateRole(RoleRequest request)
        {
            var result = new BaseResponse<bool> { Success = false };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var check = _datacontext.Roles.FirstOrDefault(s => s.SystemName == request.SystemName && s.Active && !s.UserCanDeleted);
                if (check != null)
                {
                    result.Message = "Tên hệ thống đã tồn tại";
                    result.Success = false;
                    return result;
                }
                string Permissons = "";
                List<string> RoleList = new List<string>();
                #region if - else ROLE
                if (request.ValueRole_NhomSanPham != "")
                {
                    RoleList.Add(request.ValueRole_NhomSanPham);
                }
                if (request.ValueRole_SanPham != "")
                {
                    RoleList.Add(request.ValueRole_SanPham);
                }
                if(request.ValueRole_KhachHang != "")
                {
                    RoleList.Add(request.ValueRole_KhachHang);
                }
                if(request.ValueRole_ChiNhanh != "")
                {
                    RoleList.Add(request.ValueRole_ChiNhanh);
                }
                if (request.ValueRole_BaoGia != "")
                {
                    RoleList.Add(request.ValueRole_BaoGia);
                }
                if (request.ValueRole_HopDong != "")
                {
                    RoleList.Add(request.ValueRole_HopDong);
                }
                if (request.ValueRole_TheoDoiLichHen != "")
                {
                    RoleList.Add(request.ValueRole_TheoDoiLichHen);
                }
                if (request.ValueRole_NhatKyGiaoDich != "")
                {
                    RoleList.Add(request.ValueRole_NhatKyGiaoDich);
                }
                if (request.ValueRole_ThongkeDoanhThu != "")
                {
                    RoleList.Add(request.ValueRole_ThongkeDoanhThu);
                }
                if (request.ValueRole_TonKhoHienHanh != "")
                {
                    RoleList.Add(request.ValueRole_TonKhoHienHanh);
                }
                if (request.ValueRole_BaiViet != "")
                {
                    RoleList.Add(request.ValueRole_BaiViet);
                }
                if (request.ValueRole_Video != "")
                {
                    RoleList.Add(request.ValueRole_Video);
                }
                if (request.ValueRole_AnhCongTrinh != "")
                {
                    RoleList.Add(request.ValueRole_AnhCongTrinh);
                }
                if (request.ValueRole_Catalogue != "")
                {
                    RoleList.Add(request.ValueRole_Catalogue);
                }
                if (request.ValueRole_KhoHinhAnh != "")
                {
                    RoleList.Add(request.ValueRole_KhoHinhAnh);
                }
                if (request.ValueRole_ThongKeCV != "")
                {
                    RoleList.Add(request.ValueRole_ThongKeCV);
                }
                if (request.ValueRole_DanhSachCV != "")
                {
                    RoleList.Add(request.ValueRole_DanhSachCV);
                }
                if (request.ValueRole_DanhSachCV != "")
                {
                    RoleList.Add(request.ValueRole_DanhSachCV);
                }
                if (request.ValueRole_ThongTinCaNhan != "")
                {
                    RoleList.Add(request.ValueRole_ThongTinCaNhan);
                }
                if (request.ValueRole_NgayNghi != "")
                {
                    RoleList.Add(request.ValueRole_NgayNghi);
                }
                if (request.ValueRole_ChamCong != "")
                {
                    RoleList.Add(request.ValueRole_ChamCong);
                }
                if (request.ValueRole_ViTriNV != "")
                {
                    RoleList.Add(request.ValueRole_ViTriNV);
                }
                if (request.ValueRole_NguoiDung != "")
                {
                    RoleList.Add(request.ValueRole_NguoiDung);
                }
                if (request.ValueRole_PQNguoiDung != "")
                {
                    RoleList.Add(request.ValueRole_PQNguoiDung);
                }
                if (request.ValueRole_PhanAnhKH != "")
                {
                    RoleList.Add(request.ValueRole_PhanAnhKH);
                }
                if (request.ValueRole_PhanAnhNV != "")
                {
                    RoleList.Add(request.ValueRole_PhanAnhNV);
                }
                if (request.ValueRole_LoaiThongBao != "")
                {
                    RoleList.Add(request.ValueRole_LoaiThongBao);
                }
                if (request.ValueRole_NhomThongBao != "")
                {
                    RoleList.Add(request.ValueRole_NhomThongBao);
                }
                if (request.ValueRole_ThongBao != "")
                {
                    RoleList.Add(request.ValueRole_ThongBao);
                }
                #endregion
                Permissons = string.Join(";", RoleList);
                var role = _datacontext.Roles.Add(new Role
                {
                    SystemName = request.SystemName,
                    Display = request.Display,
                    Active = true,
                    Permissons = Permissons,
                    UserCanDeleted = false,
                });
                await _datacontext.SaveChangesAsync();
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
        public async Task<BaseResponse<bool>> UpdateRole(RoleRequest request)
        {
            var result = new BaseResponse<bool> { Success = false };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var check = _datacontext.Roles.FirstOrDefault(s => s.SystemName == request.SystemName);
                if (check != null)
                {
                    string Permissons = "";
                    List<string> RoleList = new List<string>();
                    #region if - else ROLE
                    if (request.ValueRole_NhomSanPham != "")
                    {
                        RoleList.Add(request.ValueRole_NhomSanPham);
                    }
                    if (request.ValueRole_SanPham != "")
                    {
                        RoleList.Add(request.ValueRole_SanPham);
                    }
                    if (request.ValueRole_KhachHang != "")
                    {
                        RoleList.Add(request.ValueRole_KhachHang);
                    }
                    if (request.ValueRole_ChiNhanh != "")
                    {
                        RoleList.Add(request.ValueRole_ChiNhanh);
                    }
                    if (request.ValueRole_BaoGia != "")
                    {
                        RoleList.Add(request.ValueRole_BaoGia);
                    }
                    if (request.ValueRole_HopDong != "")
                    {
                        RoleList.Add(request.ValueRole_HopDong);
                    }
                    if (request.ValueRole_TheoDoiLichHen != "")
                    {
                        RoleList.Add(request.ValueRole_TheoDoiLichHen);
                    }
                    if (request.ValueRole_NhatKyGiaoDich != "")
                    {
                        RoleList.Add(request.ValueRole_NhatKyGiaoDich);
                    }
                    if (request.ValueRole_ThongkeDoanhThu != "")
                    {
                        RoleList.Add(request.ValueRole_ThongkeDoanhThu);
                    }
                    if (request.ValueRole_TonKhoHienHanh != "")
                    {
                        RoleList.Add(request.ValueRole_TonKhoHienHanh);
                    }
                    if (request.ValueRole_BaiViet != "")
                    {
                        RoleList.Add(request.ValueRole_BaiViet);
                    }
                    if (request.ValueRole_Video != "")
                    {
                        RoleList.Add(request.ValueRole_Video);
                    }
                    if (request.ValueRole_AnhCongTrinh != "")
                    {
                        RoleList.Add(request.ValueRole_AnhCongTrinh);
                    }
                    if (request.ValueRole_Catalogue != "")
                    {
                        RoleList.Add(request.ValueRole_Catalogue);
                    }
                    if (request.ValueRole_KhoHinhAnh != "")
                    {
                        RoleList.Add(request.ValueRole_KhoHinhAnh);
                    }
                    if (request.ValueRole_ThongKeCV != "")
                    {
                        RoleList.Add(request.ValueRole_ThongKeCV);
                    }
                    if (request.ValueRole_DanhSachCV != "")
                    {
                        RoleList.Add(request.ValueRole_DanhSachCV);
                    }
                    if (request.ValueRole_DanhSachCV != "")
                    {
                        RoleList.Add(request.ValueRole_DanhSachCV);
                    }
                    if (request.ValueRole_ThongTinCaNhan != "")
                    {
                        RoleList.Add(request.ValueRole_ThongTinCaNhan);
                    }
                    if (request.ValueRole_NgayNghi != "")
                    {
                        RoleList.Add(request.ValueRole_NgayNghi);
                    }
                    if (request.ValueRole_ChamCong != "")
                    {
                        RoleList.Add(request.ValueRole_ChamCong);
                    }
                    if (request.ValueRole_ViTriNV != "")
                    {
                        RoleList.Add(request.ValueRole_ViTriNV);
                    }
                    if (request.ValueRole_NguoiDung != "")
                    {
                        RoleList.Add(request.ValueRole_NguoiDung);
                    }
                    if (request.ValueRole_PQNguoiDung != "")
                    {
                        RoleList.Add(request.ValueRole_PQNguoiDung);
                    }
                    if (request.ValueRole_PhanAnhKH != "")
                    {
                        RoleList.Add(request.ValueRole_PhanAnhKH);
                    }
                    if (request.ValueRole_PhanAnhNV != "")
                    {
                        RoleList.Add(request.ValueRole_PhanAnhNV);
                    }
                    if (request.ValueRole_LoaiThongBao != "")
                    {
                        RoleList.Add(request.ValueRole_LoaiThongBao);
                    }
                    if (request.ValueRole_NhomThongBao != "")
                    {
                        RoleList.Add(request.ValueRole_NhomThongBao);
                    }
                    if (request.ValueRole_ThongBao != "")
                    {
                        RoleList.Add(request.ValueRole_ThongBao);
                    }
                    #endregion
                    Permissons = string.Join(";", RoleList);
                    check.Display = request.Display;
                    check.Active = request.ActiveRole;
                    check.Permissons = Permissons;

                    var userClaim = _datacontext.UserClaims.Where(x => x.ClaimName == check.SystemName);
                    foreach(var us in userClaim)
                    {
                        us.ClaimValue = check.Permissons;
                    }
                }
                await _datacontext.SaveChangesAsync();
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
        public async Task<BaseResponse<bool>> DeleteRole(RoleRequest request)
        {
            var result = new BaseResponse<bool> { Success = false };
            try
            {
                var role = _datacontext.Roles.FirstOrDefault(d => d.SystemName == request.SystemName);
                var userClaim = _datacontext.UserClaims.FirstOrDefault(d => d.ClaimName == request.SystemName);
                if (role != null)
                {
                    _datacontext.Roles.Remove(role);
                };
                if (userClaim != null)
                {
                    _datacontext.UserClaims.Remove(userClaim);
                };
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#else
                result.Message = ex.Message;
#endif
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<string>> lstPermisson(UserLocalRequest request)
        {
            var result = new BaseResponse<string> { Success = false };
            try
            {
                if (request.RoleName.HasValue())
                {
                    var role = await _datacontext.UserClaims.FirstOrDefaultAsync(s => s.UserId == request.UserId && s.ClaimName == request.RoleName);
                    result.Data = role?.ClaimValue.Split(';').ToList();
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                result.Message = e.ToString();
                _logService.InsertLog(e);
            }
            return result;
        }
        public async Task<BaseResponse<RoleDto>> UpdateRole(UserLocalRequest request)
        {
            var result = new BaseResponse<RoleDto> { Success = false };
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                string convertListToString = string.Join(";", request.ListRole);
                var query = await _datacontext.Roles.FirstOrDefaultAsync(s => s.SystemName == request.RoleName);
                    if(query != null)
                    {
                    query.Permissons = convertListToString;
                    await _datacontext.SaveChangesAsync();
                        result.Message = "Cập nhật thành công ";
                        result.Success = true;
                    }

            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#else
                result.Message = ex.Message;
#endif
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ChangeLockEditProduct(RoleRequest request)
        {
            var result = new BaseResponse<bool> { Success = false };
            try
            {
                var role = _datacontext.Roles.FirstOrDefault(d => d.SystemName == request.SystemName);
                if (role != null)
                {
                    role.LockEditProduct = request.LockEditProduct;
                };

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#else
                result.Message = ex.Message;
#endif
                _logService.InsertLog(ex);
            }
            return result;
        }

    }
}


