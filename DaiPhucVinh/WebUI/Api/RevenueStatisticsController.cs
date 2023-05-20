using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.RevenueStatistics;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Location;
using DaiPhucVinh.Shared.Employee;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using DaiPhucVinh.Services.MainServices.Employee;
using DaiPhucVinh.Services.MainServices.Location;
using DaiPhucVinh.Services.Framework;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/revenuestatistics")]
    public class RevenueStatisticsController : SecureApiController
    {
        private readonly IRevenueStatisticsService _revenuestatisticsService;
        private readonly ILogService _logService;
        public RevenueStatisticsController(IRevenueStatisticsService revenuestatisticsService, ILogService logService)
        {
            _revenuestatisticsService = revenuestatisticsService;
            _logService = logService;
        }
        [HttpPost]
        [Route("TakeAlls")]
        public async Task<BaseResponse<HopDongResponse>> TakeAlls([FromBody] HopDongRequest request) => await _revenuestatisticsService.TakeAlls(request);
        
        #region Export excel
        [HttpPost]
        [Route("ExportRevenueStatistic")]
        public async Task<BaseResponse<string>> ExportRevenueStatistic([FromBody] HopDongRequest request)
        {
            var result = new BaseResponse<string> { Success = false };
            try
            {
                string templateName = System.Web.HttpContext.Current.Server.MapPath("~/Exports/ExcelTemplate/ExcelTemplate_DanhSachThongkeDoanhThu.xlsx");
                result.Item = await _revenuestatisticsService.ExportRevenueStatistic(templateName, request);
                result.Success = true;
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
                _logService.InsertLog(exx);
            }
            return result;
        }
        #endregion
    }
}