using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.HopDong;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace DaiPhucVinh.Services.MainServices.HopDong
{
    public interface IHopDongService
    {
        Task<BaseResponse<HopDongResponse>> TakeAlls(HopDongRequest request);
        Task<BaseResponse<HopDong_ChiTietResponse>> TakeAllsHopDong_ChiTiet(HopDongRequest request, int Id);
        Task<BaseResponse<HopDong_TemplateResponse>> TakeAllsHopDong_Template(HopDong_TemplateRequest request);
        Task<BaseResponse<HopDongResponse>> SearchSoHopDong(string SoHopDong);
        Task<BaseResponse<bool>> Remove_Hopdong(HopDongRequest request);
        Task<BaseResponse<HopDongResponse>> Create_HopDong(HopDongRequest request);
        Task<BaseResponse<HopDongResponse>> Update_HopDong(HopDongRequest request);
        Task<BaseResponse<WordStreamHopDongResponse>> SearchData_WordStream(int HopDongId);
        Task<BaseResponse<HopDongResponse>> TakeContractByCustomerCode(string KhachHang_Code);
        Task<BaseResponse<HopDongResponse>> GetHopDongById(int Id);
        //Task<BaseResponse<HopDongResponse>> TakeQuotationById(int Id);
    }   
    public class HopDongService : IHopDongService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public HopDongService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        
        public async Task<BaseResponse<HopDongResponse>> TakeAlls(HopDongRequest request)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDongs.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.WMS_Customer.Name.Contains(request.Term) || d.WMS_Customer.Code.Contains(request.Term) || d.SoHopDong.Contains(request.Term));
                }
                if (request.KhachHang_Code.HasValue())
                {
                    query = query.Where(d => d.KhachHang_Code == request.KhachHang_Code);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.CreationDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreationDate <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<HopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDong_ChiTietResponse>> TakeAllsHopDong_ChiTiet(HopDongRequest request, int Id)
        {
            var result = new BaseResponse<HopDong_ChiTietResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDong_ChiTiets.Where(x => x.HopDong_Id == Id).AsQueryable();
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.OrderBy).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<HopDong_ChiTietResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> TakeQuotation(string KhachHang_Code)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = await _datacontext.WMS_Quotations.AsNoTracking().Where(code => code.CustomerCode == KhachHang_Code).ToListAsync();
                result.Data = query.MapTo<HopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDong_TemplateResponse>> TakeAllsHopDong_Template(HopDong_TemplateRequest request)
        {
            var result = new BaseResponse<HopDong_TemplateResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDong_Templetes.AsQueryable().ToList();
                result.Data = query.MapTo<HopDong_TemplateResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> SearchSoHopDong(string SoHopDong)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = await _datacontext.WMS_HopDongs.FirstOrDefaultAsync(x => x.SoHopDong.ToLower() == SoHopDong.ToLower());
                result.Item = query.MapTo<HopDongResponse>();
                if (query == null)
                {
                    result.Success = false;
                }
                else
                {
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> SearchEmployee(string EmployeeCode)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var query = await _datacontext.WMS_Employees.SingleOrDefaultAsync(x => x.EmployeeCode == EmployeeCode);
                result.Item = query.MapTo<EmployeeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> Remove_Hopdong(HopDongRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var hopdong = await _datacontext.WMS_HopDongs.SingleOrDefaultAsync(x => x.Id == request.Id);
                var transaction = await _datacontext.WMS_TransactionInformations.Where(x => x.HopDong_Id == hopdong.Id).ToListAsync();
                if (transaction != null)
                {
                    _datacontext.WMS_TransactionInformations.RemoveRange(transaction);
                }
                var hopdongct = await _datacontext.WMS_HopDong_ChiTiets.Where(x => x.HopDong_Id == hopdong.Id).ToListAsync();
                if(hopdongct != null)
                {
                    _datacontext.WMS_HopDong_ChiTiets.RemoveRange(hopdongct);
                }
                _datacontext.WMS_HopDongs.Remove(hopdong);
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
        public async Task<BaseResponse<HopDongResponse>> Create_HopDong(HopDongRequest request)
         {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                DateTime myDate = DateTime.Parse("12/31/0001 12:00:00 AM");
                DateTime myDate1 = DateTime.Parse("1/1/0001 12:00:00 AM");
                if (request.NgayKy == myDate)
                {
                    request.NgayKy = null;
                }
                if (request.NgayKy == myDate1)
                {
                    request.NgayKy = null;
                }
                if (request.NgayChuyenTien == myDate)
                {
                    request.NgayChuyenTien = null;
                }
                if (request.NgayChuyenTien == myDate1)
                {
                    request.NgayChuyenTien = null;
                }
                if (request.NgayGiaoHang == myDate)
                {
                    request.NgayGiaoHang = null;
                }
                if (request.NgayGiaoHang == myDate1)
                {
                    request.NgayGiaoHang = null;
                }
                if (request.QuatationCode == 0)
                {
                    request.QuatationCode = null;
                }
                //Create
                var hopdong = _datacontext.WMS_HopDongs.Add(new WMS_HopDong
                    {
                        KhachHang_Code = request.CustomerCode,
                        LocationCode = request.LocationCode,
                        BaoGia_Id = request.QuatationCode,
                        SoHopDong = request.ContractNumber,
                        NgayKy = request.NgayKy,
                        BenA_TenCongTy = request.BenA_Name,
                        BenA_DiaChi = request.BenA_Address,
                        BenA_SoDienThoai = request.BenA_Phone,
                        BenA_Fax = request.BenA_Fax,
                        BenA_MST = request.BenA_MST,
                        BenA_SoTaiKhoanNganHang = request.BenA_Banking,
                        BenA_NguoiDaiDien = request.BenA_ManagerName,
                        BenA_ChucVu =  request.BenA_ManagerRole,
                        BenB_TenCongTy = request.BenB_Name,
                        BenB_DiaChi = request.BenB_Address,
                        BenB_SoDienThoai = request.BenB_Phone,
                        BenB_Fax = request.BenB_Fax,
                        BenB_MST = request.BenB_MST,
                        BenB_SoTaiKhoanNganHang = request.BenB_Banking,
                        BenB_NguoiDaiDien = request.BenB_ManagerName,
                        BenB_ChucVu = request.BenB_ManagerRole,
                        TongTien_TruocThue = request.TongTien,
                        PhanTramThue = request.PhanTramThue,
                        TienThue = request.Thue,
                        TongTien = request.TienSauThue,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        CreationDate = DateTime.Now,
                        LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                        LastUpdateDate = DateTime.Now,
                        ChuyenTien1_Ngay = request.NgayChuyenTien,
                        ChuyenTien1_SoTien = request.ChuyenTienDot1,
                        ChuyenTien2_SoTien = request.ChuyenTienDot2,
                        ChuyenTien3_SoTien = request.ChuyenTienDot3,
                        Dieu2 = request.Dieu2,
                        Dieu3 = request.Dieu3,
                        Dieu4 = request.Dieu4,
                        Dieu5 = request.Dieu5,
                        Dieu6 = request.Dieu6,
                        NgayBatDauGiaoHang = request.NgayGiaoHang,
                        IsDaKy = true,
                        NgayTaoHopDong = DateTime.Now,
                        EmployeeCode = request.EmployeeCode,
                        SoTienConLaiSauThanhToan = request.TienSauThue,
                    });
                    foreach (var line in request.dataItemCheck)
                    {
                        _datacontext.WMS_HopDong_ChiTiets.Add(new WMS_HopDong_ChiTiet
                        {
                           HopDong_Id = hopdong.Id,
                            ItemCode = line.Code,
                            DonGia = line.price,
                            ThanhTien = line.amt,
                            SoLuong = line.qty,
                            OrderBy = line.OrderBy,
                        });
                    }
                    if(request.NgayGiaoHang.HasValue && request.NgayGiaoHang.Value > DateTime.MinValue)
                    {
                        _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                        {
                            TransactionType_Id = 3,
                            Date = DateTime.Now,
                            Year = DateTime.Now.Year,
                            CustomerCode = request.CustomerCode,
                            LocationCode = request.LocationCode,
                            BeginTime = request.NgayTaoHopDong,
                            NgayGiaoHang = request.NgayGiaoHang,
                            NgayChuyenTien = request.NgayChuyenTien,
                            Description = string.Format("Giao hàng: HĐ số {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                            Appointment_Description = string.Format("Giao hàng: HĐ số {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                            EmployeeCode = request.EmployeeCode,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                            CreationDate = DateTime.Now,
                            AppointmentDate = request.NgayKy,
                            DanhGia_Id = 1,
                            HopDong_Id = hopdong.Id,

                        });
                    }
                    if (request.NgayChuyenTien.HasValue && request.NgayChuyenTien.Value > DateTime.MinValue)
                    {
                        _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                        {
                            TransactionType_Id = 3,
                            Date = DateTime.Now,
                            Year = DateTime.Now.Year,
                            CustomerCode = request.CustomerCode,
                            LocationCode = request.LocationCode,
                            BeginTime = request.NgayTaoHopDong,
                            NgayGiaoHang = request.NgayGiaoHang,
                            NgayChuyenTien = request.NgayChuyenTien,
                            Description = string.Format("Chuyển tiền HĐ số  {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                            Appointment_Description = string.Format("Chuyển tiền HĐ số  {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                            EmployeeCode = request.EmployeeCode,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                            CreationDate = DateTime.Now,
                            AppointmentDate = request.NgayKy,
                            DanhGia_Id = 1,
                            HopDong_Id = hopdong.Id,

                        });
                    }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
                result.Item = hopdong.MapTo<HopDongResponse>();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> Update_HopDong(HopDongRequest request)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                DateTime myDate = DateTime.Parse("12/31/0001 12:00:00 AM");
                DateTime myDate1 = DateTime.Parse("1/1/0001 12:00:00 AM");
                if (request.NgayKy == myDate)
                {
                    request.NgayKy = null;
                }
                if (request.NgayKy == myDate1)
                {
                    request.NgayKy = null;
                }
                if (request.NgayChuyenTien == myDate)
                {
                    request.NgayChuyenTien = null;
                }
                if (request.NgayChuyenTien == myDate1)
                {
                    request.NgayChuyenTien = null;
                }
                if (request.NgayGiaoHang == myDate)
                {
                    request.NgayGiaoHang = null;
                }
                if (request.NgayGiaoHang == myDate1)
                {
                    request.NgayGiaoHang = null;
                }
                         //Edit
                    var hopdong = await _datacontext.WMS_HopDongs.SingleOrDefaultAsync(x => x.Id == request.Id);
                    hopdong.KhachHang_Code = request.CustomerCode;
                    hopdong.LocationCode = request.LocationCode;
                    hopdong.BaoGia_Id = request.QuatationCode;
                    hopdong.SoHopDong = request.ContractNumber;
                    hopdong.NgayKy = request.NgayKy;
                    hopdong.BenA_TenCongTy = request.BenA_Name;
                    hopdong.BenA_DiaChi = request.BenA_Address;
                    hopdong.BenA_SoDienThoai = request.BenA_Phone;
                    hopdong.BenA_Fax = request.BenA_Fax;
                    hopdong.BenA_MST = request.BenA_MST;
                    hopdong.BenA_SoTaiKhoanNganHang = request.BenA_Banking;
                    hopdong.BenA_NguoiDaiDien = request.BenA_ManagerName;
                    hopdong.BenA_ChucVu = request.BenA_ManagerRole;
                    hopdong.BenB_TenCongTy = request.BenB_Name;
                    hopdong.BenB_DiaChi = request.BenB_Address;
                    hopdong.BenB_SoDienThoai = request.BenB_Phone;
                    hopdong.BenB_Fax = request.BenB_Fax;
                    hopdong.BenB_MST = request.BenB_MST;
                    hopdong.BenB_SoTaiKhoanNganHang = request.BenB_Banking;
                    hopdong.BenB_NguoiDaiDien = request.BenB_ManagerName;
                    hopdong.BenB_ChucVu = request.BenB_ManagerRole;
                    hopdong.TongTien_TruocThue = request.TongTien;
                    hopdong.PhanTramThue = request.PhanTramThue;
                    hopdong.TienThue = request.Thue;
                    hopdong.TongTien = request.TienSauThue;
                    hopdong.Dieu2 = request.Dieu2;
                    hopdong.Dieu3 = request.Dieu3;
                    hopdong.Dieu4 = request.Dieu4;
                    hopdong.Dieu5 = request.Dieu5;
                    hopdong.Dieu6 = request.Dieu6;
                    hopdong.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    hopdong.LastUpdateDate = DateTime.Now;
                    hopdong.ChuyenTien1_Ngay = request.NgayChuyenTien;
                    hopdong.ChuyenTien1_SoTien = request.ChuyenTienDot1;
                    hopdong.ChuyenTien2_SoTien = request.ChuyenTienDot2;
                    hopdong.ChuyenTien3_SoTien = request.ChuyenTienDot3;
                    hopdong.NgayBatDauGiaoHang = request.NgayGiaoHang;
                    hopdong.IsDaKy = true;
                    hopdong.NgayTaoHopDong = request.NgayTaoHopDong;
                    hopdong.EmployeeCode = request.EmployeeCode;
                    hopdong.SoTienConLaiSauThanhToan = request.TienSauThue;
                    var HopDongCT = await _datacontext.WMS_HopDong_ChiTiets.Where(x => x.HopDong_Id == hopdong.Id).ToListAsync();
                    _datacontext.WMS_HopDong_ChiTiets.RemoveRange(HopDongCT);
                    foreach (var line in request.dataItemCheck)
                    {
                        _datacontext.WMS_HopDong_ChiTiets.Add(new WMS_HopDong_ChiTiet
                        {
                            HopDong_Id = hopdong.Id,
                            ItemCode = line.Code,
                            DonGia = line.price,
                            ThanhTien = line.amt,
                            SoLuong = line.qty,
                            OrderBy = line.OrderBy,
                        });
                    }
                    var transaction = await _datacontext.WMS_TransactionInformations.Where(x => x.HopDong_Id == hopdong.Id).ToListAsync();
                    if (transaction != null)
                    {
                        _datacontext.WMS_TransactionInformations.RemoveRange(transaction);

                        if (request.NgayGiaoHang.HasValue && request.NgayGiaoHang.Value > DateTime.MinValue)
                        {
                            _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = 3,
                                Date = DateTime.Now,
                                Year = DateTime.Now.Year,
                                CustomerCode = request.CustomerCode,
                                LocationCode = request.LocationCode,
                                BeginTime = request.NgayTaoHopDong,
                                NgayGiaoHang = request.NgayGiaoHang,
                                NgayChuyenTien = request.NgayChuyenTien,
                                Description = string.Format("Giao hàng: HĐ số {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                                Appointment_Description = string.Format("Giao hàng: HĐ số {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                AppointmentDate = request.NgayKy,
                                DanhGia_Id = 1,
                                HopDong_Id = hopdong.Id,

                            });
                        }
                        if (request.NgayChuyenTien.HasValue && request.NgayChuyenTien.Value > DateTime.MinValue)
                        {
                            _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = 3,
                                Date = DateTime.Now,
                                Year = DateTime.Now.Year,
                                CustomerCode = request.CustomerCode,
                                LocationCode = request.LocationCode,
                                BeginTime = request.NgayTaoHopDong,
                                NgayGiaoHang = request.NgayGiaoHang,
                                NgayChuyenTien = request.NgayChuyenTien,
                                Description = string.Format("Chuyển tiền HĐ số {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                                Appointment_Description = string.Format("Chuyển tiền HĐ số  {0}; {1}; ký ngày {2}", request.ContractNumber, request.BenA_Name, request.NgayKy.HasValue ? request.NgayKy.Value.ToShortDateString() : string.Empty),
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                AppointmentDate = request.NgayKy,
                                DanhGia_Id = 1,
                                HopDong_Id = hopdong.Id,
                            });
                        }
                    }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
                result.Item = hopdong.MapTo<HopDongResponse>();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<WordStreamHopDongResponse>> SearchData_WordStream(int HopDongId)
        {
            var result = new BaseResponse<WordStreamHopDongResponse> { };
            try
            {
                if (HopDongId == 0)
                {
                    result.Message = "Không tìm thấy phiếu báo giá";
                    return result;
                }
                var query = await _datacontext.WMS_HopDongs.FindAsync(HopDongId);
                result.Item = query.MapTo<WordStreamHopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> TakeContractByCustomerCode(string KhachHang_Code)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDongs.AsNoTracking().Where(code => code.KhachHang_Code == KhachHang_Code).ToList();
                result.Data = query.MapTo<HopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> GetHopDongById(int Id)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = await _datacontext.WMS_HopDongs.FindAsync(Id);
                result.Item = query.MapTo<HopDongResponse>();
                result.Success = true;
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
