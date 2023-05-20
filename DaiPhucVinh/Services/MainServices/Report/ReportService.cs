using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Report;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Report
{
    public interface IReportService
    {
        Task<ReportResponse> ReportTakeAlls(ReportRequest request);
    }
    public class ReportService : IReportService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public ReportService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }

        public async Task<ReportResponse> ReportTakeAlls(ReportRequest request)
        {
            var result = new ReportResponse { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var task = _datacontext.FUV_Tasks.AsQueryable().Where(s => !s.Deleted && s.AssignUserId == UserId);
                var task_HT = task.Where(x => x.TaskResultId != null && x.TaskFinishAt != null);
                var task_CHT = task.Where(x => x.TaskResultId == null && x.TaskFinishAt == null);
                var attendance = _datacontext.FUV_Attendances.AsQueryable().Where(r => r.UserId == UserId && r.CheckInData != null && r.CheckOutData != null);
                var dayoff = _datacontext.FUV_Dayoffs.AsQueryable().Where(r => r.UserId == UserId && r.Approved);
                var feedback = _datacontext.FUV_Feedbacks.AsQueryable().Where(r => r.UserId == UserId && r.Approved);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    task = task.Where(x => _FromDt <= x.StartDate);
                    task_HT = task_HT.Where(x => _FromDt <= x.StartDate);
                    task_CHT = task_CHT.Where(x => _FromDt <= x.StartDate);
                    attendance = attendance.Where(x => _FromDt <= x.WorkDate);
                    dayoff = dayoff.Where(x => _FromDt <= x.StartDate);
                    feedback = feedback.Where(x => _FromDt <= x.CreatedAt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    task = task.Where(x => x.StartDate <= _ToDt);
                    task_HT = task_HT.Where(x => x.StartDate <= _ToDt);
                    task_CHT = task_CHT.Where(x => x.StartDate <= _ToDt);
                    attendance = attendance.Where(x => x.WorkDate <= _ToDt);
                    dayoff = dayoff.Where(x => x.StartDate <= _ToDt);
                    feedback = feedback.Where(x => x.CreatedAt <= _ToDt);
                }
                foreach (var taskx in task_HT)
                {
                    double x = double.Parse(taskx.Distance.Replace("km", ""));
                    result.SoKm = result.SoKm + x;
                }
                result.SLCongViec = await task.CountAsync();
                result.CV_HoanThanh = await task_HT.CountAsync();
                result.CV_ChuaHoanThanh = await task_CHT.CountAsync();
                result.SLChamCong = await attendance.CountAsync();
                result.SLNgayNghi = await dayoff.CountAsync();
                result.SLDeXuat = await feedback.CountAsync();
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
