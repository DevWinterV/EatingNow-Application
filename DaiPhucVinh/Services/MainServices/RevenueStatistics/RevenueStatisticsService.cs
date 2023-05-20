using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Location;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using DaiPhucVinh.Services.Excel;
using System.Threading.Tasks;
using Falcon.Core;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Server.Data.DaiPhucVinh;

namespace DaiPhucVinh.Services.MainServices.RevenueStatistics
{
    public interface IRevenueStatisticsService
    {
        Task<BaseResponse<HopDongResponse>> TakeAlls(HopDongRequest request);
        Task<string> ExportRevenueStatistic(string templateName, HopDongRequest request);

    }
    public class RevenueStatisticsService : IRevenueStatisticsService
    {
        private readonly DataContext _datacontext;
        private readonly IExportService _exportService;
        private readonly ILogService _logService;

        public RevenueStatisticsService(DataContext datacontext, IExportService exportService, ILogService logService)
        {
            _datacontext = datacontext;
            _exportService = exportService;
            _logService = logService;

        }
        public async Task<BaseResponse<HopDongResponse>> TakeAlls(HopDongRequest request)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDongs.AsQueryable();

                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.NgayTaoHopDong);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.NgayTaoHopDong <= _ToDt);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                //if (request.FromDt.HasValue)
                //{
                //    query = query.Where(d => d.NgayKy > request.FromDt);
                //}
                //if (request.ToDt.HasValue)
                //{
                //    query = query.Where(d => d.NgayKy <= request.ToDt);
                //}
                result.DataCount = await query.CountAsync();
                result.CustomData = new
                {
                    TongTien = await query.SumAsync(d => d.TongTien)
                };
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
        public async Task<string> ExportRevenueStatistic(string templateName, HopDongRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            request.Page = 0;
            request.PageSize = int.MaxValue;
            var datas = await TakeAlls(request);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _exportService.ExportToXlsxRevenueStatistic(stream, templateName, datas, request);
                bytes = stream.ToArray();
            }
            string nameFileExport = $"ThongKeDoanhThu_{DateTime.Now.Ticks}.xlsx";
            string downloadFolder = "/download/" + request.EmployeeName;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            File.WriteAllBytes(Path.Combine(exportFilePath), bytes);
            await _datacontext.SaveChangesAsync();
            return downloadFolder + "/" + nameFileExport;
        }

    }
}
