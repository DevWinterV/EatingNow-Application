    using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.TransactionInformation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.TransactionInformation
{
    public interface ITransactionInformationService
    {
        Task<BaseResponse<TransactionInformationResponse>> TakeAllTransactionInformation(TransactionInformationRequest request);
        Task<BaseResponse<TransactionModalResponse>> TakeTransactionInformationById(int Id);
        Task<BaseResponse<bool>> CreateTransactionInformation(TransactionInformationRequest request);
        Task<BaseResponse<bool>> UpdateTransactionInformation(TransactionInformationRequest request);
        Task<BaseResponse<bool>> DeleteTransactionInformation(TransactionInformationRequest request);
        Task<BaseResponse<AppointmentTrackingResponse>> TakeTransactionInformationForAppointmentTracking(AppointmentTrackingRequest request);
        Task<BaseResponse<TransactionHistoryTypeResponse>> TakeAllTransactionType(TransactionInformationRequest request);
    }

    public class TransactionInformationService : ITransactionInformationService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public TransactionInformationService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<TransactionHistoryTypeResponse>> TakeAllTransactionType(TransactionInformationRequest request)
        {
            var result = new BaseResponse<TransactionHistoryTypeResponse> { };
            try
            {
                var query = _datacontext.WMS_TransactionTypes.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<TransactionHistoryTypeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<TransactionInformationResponse>> TakeAllTransactionInformation(TransactionInformationRequest request)
        {
            var result = new BaseResponse<TransactionInformationResponse> { };
            try
            {
                var query = _datacontext.WMS_TransactionInformations.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.WMS_Customer.Name.Contains(request.Term) || d.WMS_Customer.PhoneNo.Contains(request.Term));
                }
                if (request.TransactionTypeId > 0)
                {
                    query = query.Where(d => d.TransactionType_Id == request.TransactionTypeId);
                }
                if (request.ProvinceId > 0)
                {
                    query = query.Where(d => d.WMS_Customer.TinhThanh_Id == request.ProvinceId);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<TransactionInformationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<TransactionModalResponse>> TakeTransactionInformationById(int Id)
        {
            var result = new BaseResponse<TransactionModalResponse> { };
            try
            {
                var data = await _datacontext.WMS_TransactionInformations.FindAsync(Id);
                result.Item = data.MapTo<TransactionModalResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateTransactionInformation(TransactionInformationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {
                    //Create
                    if(request.TransactionTypeId == 1) {
                        var transactioninformation = _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                        {
                            TransactionType_Id = request.TransactionTypeId,
                            Date = request.Date,
                            ContacPerson = request.ContacPerson,
                            Year = request.Year,
                            CustomerCode = request.CustomerCode,
                            BeginTime = request.Date,
                            Description = request.Description,
                            EmployeeCode = request.EmployeeCode,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                            CreationDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now,
                            LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                            Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1,
                            
                            //FileAttach
                            AppointmentDate = request.AppointmentDate,
                            Appointment_Description = request.Appointment_Description,
                            DanhGia_Id = request.DanhGia_Id,
                            BaoGia_Id = null,
                            HopDong_Id = null,

                        });
                    }
                    else if(request.TransactionTypeId == 2)
                    {
                        if (request.BaoGia_Id == 0)
                        {
                            var transactioninformation = _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = request.TransactionTypeId,
                                Date = request.Date,
                                ContacPerson = request.ContacPerson,
                                Year = request.Year,
                                CustomerCode = request.CustomerCode,
                                BeginTime = request.Date,
                                Description = request.Description,
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1,
                                // CreateBy, CreateDate, LastUpdateBy, LastUpdateDate ...
                                //FileAttach
                                AppointmentDate = request.AppointmentDate,
                                Appointment_Description = request.Appointment_Description,
                                DanhGia_Id = request.DanhGia_Id,
                                BaoGia_Id = null,
                                HopDong_Id = null,

                            });
                        }
                        else
                        {
                            var transactioninformation = _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = request.TransactionTypeId,
                                Date = request.Date,
                                ContacPerson = request.ContacPerson,
                                Year = request.Year,
                                CustomerCode = request.CustomerCode,
                                BeginTime = request.Date,
                                Description = request.Description,
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1,
                                // CreateBy, CreateDate, LastUpdateBy, LastUpdateDate ...
                                //FileAttach
                                AppointmentDate = request.AppointmentDate,
                                Appointment_Description = request.Appointment_Description,
                                DanhGia_Id = request.DanhGia_Id,
                                BaoGia_Id = request.BaoGia_Id,
                                HopDong_Id = null,

                            });
                        }
                        
                    }
                    else
                    {
                        if(request.HopDong_Id == 0)
                        {
                            var transactioninformation = _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = request.TransactionTypeId,
                                Date = request.Date,
                                ContacPerson = request.ContacPerson,
                                Year = request.Year,
                                CustomerCode = request.CustomerCode,
                                BeginTime = request.Date,
                                Description = request.Description,
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1,
                                // CreateBy, CreateDate, LastUpdateBy, LastUpdateDate ...
                                //FileAttach
                                AppointmentDate = request.AppointmentDate,
                                Appointment_Description = request.Appointment_Description,
                                DanhGia_Id = request.DanhGia_Id,
                                BaoGia_Id = null,
                                HopDong_Id = null,
                                NgayChuyenTien = request.NgayChuyenTien,
                                NgayGiaoHang = request.NgayGiaoHang,

                            });
                        }
                        else
                        {
                            var transactioninformation = _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                            {
                                TransactionType_Id = request.TransactionTypeId,
                                Date = request.Date,
                                ContacPerson = request.ContacPerson,
                                Year = request.Year,
                                CustomerCode = request.CustomerCode,
                                BeginTime = request.Date,
                                Description = request.Description,
                                EmployeeCode = request.EmployeeCode,
                                CreatedBy = TokenHelper.CurrentIdentity().UserName,
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1,
                                // CreateBy, CreateDate, LastUpdateBy, LastUpdateDate ...
                                //FileAttach
                                AppointmentDate = request.AppointmentDate,
                                Appointment_Description = request.Appointment_Description,
                                DanhGia_Id = request.DanhGia_Id,
                                BaoGia_Id = null,
                                HopDong_Id = request.HopDong_Id,
                                NgayChuyenTien = request.NgayChuyenTien,
                                NgayGiaoHang = request.NgayGiaoHang,
                            });
                        }
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
        public async Task<BaseResponse<bool>> UpdateTransactionInformation(TransactionInformationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    //Edit
                    if (request.TransactionTypeId == 1)
                    {
                        var transactioninformation = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                        transactioninformation.TransactionType_Id = request.TransactionTypeId;
                        transactioninformation.Date = request.Date;
                        transactioninformation.Year = request.Year;
                        transactioninformation.Description = request.Description;
                        transactioninformation.EmployeeCode = request.EmployeeCode;
                        transactioninformation.DanhGia_Id = request.DanhGia_Id;
                        transactioninformation.AppointmentDate = request.AppointmentDate;
                        transactioninformation.Appointment_Description = request.Appointment_Description;
                        transactioninformation.LastUpdateDate = DateTime.Now;
                        transactioninformation.BaoGia_Id = null;
                        transactioninformation.HopDong_Id = null;
                        transactioninformation.Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1;
                    }
                    else if (request.TransactionTypeId == 2)
                    {

                        if (request.BaoGia_Id == 0)
                        {
                            var transactioninformation = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                            transactioninformation.TransactionType_Id = request.TransactionTypeId;
                            transactioninformation.Date = request.Date;
                            transactioninformation.Year = request.Year;
                            transactioninformation.EmployeeCode = request.EmployeeCode;
                            transactioninformation.Description = request.Description;
                            transactioninformation.DanhGia_Id = request.DanhGia_Id;
                            transactioninformation.AppointmentDate = request.AppointmentDate;
                            transactioninformation.Appointment_Description = request.Appointment_Description;
                            transactioninformation.LastUpdateDate = DateTime.Now;
                            transactioninformation.BaoGia_Id = null;
                            transactioninformation.HopDong_Id = null;
                            transactioninformation.Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1;
                        }
                        else
                        {
                            var transactioninformation = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                            transactioninformation.TransactionType_Id = request.TransactionTypeId;
                            transactioninformation.Date = request.Date;
                            transactioninformation.Year = request.Year;
                            transactioninformation.Description = request.Description;
                            transactioninformation.EmployeeCode = request.EmployeeCode;
                            transactioninformation.DanhGia_Id = request.DanhGia_Id;
                            transactioninformation.AppointmentDate = request.AppointmentDate;
                            transactioninformation.Appointment_Description = request.Appointment_Description;
                            transactioninformation.LastUpdateDate = DateTime.Now;
                            transactioninformation.BaoGia_Id = request.BaoGia_Id;
                            transactioninformation.HopDong_Id = null;
                            transactioninformation.Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1;
                        }

                    }
                    else
                    {
                        if (request.HopDong_Id == 0)
                        {
                            var transactioninformation = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                            transactioninformation.TransactionType_Id = request.TransactionTypeId;
                            transactioninformation.Date = request.Date;
                            transactioninformation.Year = request.Year;
                            transactioninformation.EmployeeCode = request.EmployeeCode;
                            transactioninformation.Description = request.Description;
                            transactioninformation.DanhGia_Id = request.DanhGia_Id;
                            transactioninformation.AppointmentDate = request.AppointmentDate;
                            transactioninformation.Appointment_Description = request.Appointment_Description;
                            transactioninformation.LastUpdateDate = DateTime.Now;
                            transactioninformation.BaoGia_Id = null;
                            transactioninformation.HopDong_Id = null;
                            transactioninformation.NgayChuyenTien = request.NgayChuyenTien;
                            transactioninformation.NgayGiaoHang = request.NgayGiaoHang;
                            transactioninformation.Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1;
                        }
                        else
                        {
                            var transactioninformation = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                            transactioninformation.TransactionType_Id = request.TransactionTypeId;
                            transactioninformation.Date = request.Date;
                            transactioninformation.Year = request.Year;
                            transactioninformation.Description = request.Description;
                            transactioninformation.EmployeeCode = request.EmployeeCode;
                            transactioninformation.DanhGia_Id = request.DanhGia_Id;
                            transactioninformation.AppointmentDate = request.AppointmentDate;
                            transactioninformation.Appointment_Description = request.Appointment_Description;
                            transactioninformation.LastUpdateDate = DateTime.Now;
                            transactioninformation.BaoGia_Id = null;
                            transactioninformation.HopDong_Id = request.HopDong_Id;
                            transactioninformation.NgayChuyenTien = request.NgayChuyenTien;
                            transactioninformation.NgayGiaoHang = request.NgayGiaoHang;
                            transactioninformation.Is_DaLienHeLai_Lan1 = request.Is_DaLienHeLai_Lan1;
                        }

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
        public async Task<BaseResponse<bool>> DeleteTransactionInformation(TransactionInformationRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                var entity = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.Id == request.Id);
                _datacontext.WMS_TransactionInformations.Remove(entity);
              
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<AppointmentTrackingResponse>> TakeTransactionInformationForAppointmentTracking(AppointmentTrackingRequest request)
        {
            var result = new BaseResponse<AppointmentTrackingResponse> { };
            try
            {
                var query = _datacontext.WMS_TransactionInformations.AsQueryable();
                query = query.Where(x => x.Is_DaLienHeLai_Lan1 != true);
                query = query.Where(x => x.AppointmentDate.HasValue);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.Date);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date;
                    query = query.Where(x => x.AppointmentDate <= _ToDt);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.AppointmentDate); 
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AppointmentTrackingResponse>();
                var groupZero = result.Data.Where(d => d.DayRemain == 0).ToList();
                var groupGreater = result.Data.Where(d => d.DayRemain > 0).OrderBy(d => d.DayRemain).ToList();
                var groupLess = result.Data.Where(d => d.DayRemain < 0).OrderByDescending(d => d.DayRemain).ToList();
                List<AppointmentTrackingResponse> lastlist = new List<AppointmentTrackingResponse>();
                lastlist.AddRange(groupZero);
                lastlist.AddRange(groupGreater);
                lastlist.AddRange(groupLess);

                List<AppointmentTrackingResponse> listAfterPaging = new List<AppointmentTrackingResponse> (lastlist.Skip(request.Page * request.PageSize).Take(request.PageSize));
                
                result.Data = listAfterPaging;

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
