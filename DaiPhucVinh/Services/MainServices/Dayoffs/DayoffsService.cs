using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Excel;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Chart;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Dayoffs;
using DaiPhucVinh.Shared.User;
using Falcon.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Dayoffs
{
    public interface IDayoffsService
    {
        Task<BaseResponse<DayoffsResponse>> TakeAlls(DayoffsRequest request);
        Task<BaseResponse<DayoffsResponse>> TakeAllNotPaging(DayoffsRequest request);
        Task<BaseResponse<DayoffsResponse>> TakeAllDayOff_ByUserId(DayoffsRequest request);
        Task<BaseResponse<bool>> CreateDayOff(DayoffsRequest request);
        Task<BaseResponse<bool>> EditDayOff(DayoffsRequest request);
        Task<BaseResponse<bool>> RemoveDayOff(DayoffsRequest request);
        Task<BaseResponse<bool>> Update_Dayoffs(DayoffsRequest request);
        Task<BaseResponse<DayoffsResponse>> Deleted(DayoffsRequest request);
        Task<string> ExportDayoffs(string templateName, DayoffsRequest request);
    }
    public class DayoffsService : IDayoffsService
    {
        private readonly DataContext _datacontext;
        private readonly IExportService _exportService;
        private readonly ILogService _logService;
        public DayoffsService(DataContext datacontext, IExportService exportService, ILogService logService)
        {
            _exportService = exportService;
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<DayoffsResponse>> TakeAlls(DayoffsRequest request)
        {
            var result = new BaseResponse<DayoffsResponse> { };
            try
            {
                var query = _datacontext.FUV_Dayoffs.AsQueryable().Where(r => !r.Deleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.User.DisplayName.Contains(request.Term) );
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.StartDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(d => d.StartDate <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.StartDate).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DayoffsResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<DayoffsResponse>> TakeAllNotPaging(DayoffsRequest request)
        {
            var result = new BaseResponse<DayoffsResponse> { };
            try
            {
                var query = _datacontext.FUV_Dayoffs.AsQueryable().Where(r => !r.Deleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.User.DisplayName.Contains(request.Term));
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.FromDt.HasValue)
                {
                    query = query.Where(d => d.StartDate > request.FromDt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(d => d.StartDate <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DayoffsResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<DayoffsResponse>> TakeAllDayOff_ByUserId(DayoffsRequest request)
        {
            var result = new BaseResponse<DayoffsResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Dayoffs.AsQueryable().Where(r => r.UserId == UserId && !r.Deleted);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.UpdatedAt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.UpdatedAt <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.UpdatedAt).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DayoffsResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateDayOff(DayoffsRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var dayoffs = _datacontext.FUV_Dayoffs.Add(new FUV_Dayoffs
                {
                    StartDate = request.StartDate,
                    DayRequests = request.DayRequests,
                    Description = request.Description,
                    Approved = false,
                    UserId = TokenHelper.CurrentIdentity().UserId,
                    CreatedAt = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    Deleted = false,
                });
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
        public async Task<BaseResponse<bool>> EditDayOff(DayoffsRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Dayoffs.FindAsync(request.Id);
                entity.StartDate = request.StartDate;
                entity.DayRequests = request.DayRequests;
                entity.Description = request.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = TokenHelper.CurrentIdentity().UserName;

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
        public async Task<BaseResponse<bool>> RemoveDayOff(DayoffsRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Dayoffs.FindAsync(request.Id);
                entity.Deleted = true;
  
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
        // Sửa lại thành ManagerApprove
        public async Task<BaseResponse<bool>> Update_Dayoffs(DayoffsRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if(request.Id > 0)
                {
                    var dayoffs = await _datacontext.FUV_Dayoffs.SingleOrDefaultAsync(x => x.Id == request.Id);
                    dayoffs.Approved = request.Approved;
                    dayoffs.ApprovedDescription = request.ApprovedDescription;
                    dayoffs.DayApproved = request.DayApproved;
                    dayoffs.UpdatedAt = DateTime.Now;
                    dayoffs.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                }
              
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch(Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<DayoffsResponse>> Deleted (DayoffsRequest request)
        {
            var result = new BaseResponse<DayoffsResponse> { };
            try
            {
                var query = await _datacontext.FUV_Dayoffs.FindAsync(request.Id);
                query.Deleted = true;
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
        public async Task<BaseResponse<DayoffsResponse>> TakeExport(DayoffsRequest request)
        {
            var result = new BaseResponse<DayoffsResponse> { };
            try
            {
                var query = _datacontext.FUV_Dayoffs.AsQueryable().Where(x => !x.Deleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.User.DisplayName.Contains(request.Term));
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.StartDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.StartDate <= _ToDt);
                }

                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id);
                request.Page = 0;
                request.PageSize = int.MaxValue;
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DayoffsResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<string> ExportDayoffs(string templateName, DayoffsRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            var datas = await TakeExport(request);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _exportService.ExportToXlsxDayoffs(stream, templateName, datas, request);
                bytes = stream.ToArray();
            }
            string nameFileExport = $"DanhSachQLNgayNghi_{DateTime.Now.Ticks}.xlsx";
            string downloadFolder = "/download/" + "QLNgayNghi";
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            File.WriteAllBytes(Path.Combine(exportFilePath), bytes);
            return downloadFolder + "/" + nameFileExport;
        }
    }
}
