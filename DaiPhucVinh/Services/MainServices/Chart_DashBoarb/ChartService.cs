using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Chart;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Chart_DashBoarb
{
    public interface IChartService
    {
        Task<BaseResponse<RevenueResponse>> TotalRevenue_Chart(ChartRequest request);
        Task<BaseResponse<PriceQuoteResponse>> TotalPriceQuote(ChartRequest request);
    }
    public class ChartService : IChartService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public ChartService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }

        public async Task<BaseResponse<RevenueResponse>> TotalRevenue_Chart(ChartRequest request)
        {
            var result = new BaseResponse<RevenueResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDongs.AsQueryable();
                if (request.FromDt.HasValue)
                {
                    query = query.Where(d => d.NgayKy > request.FromDt);
                }
                if (request.ToDt.HasValue)
                {
                    query = query.Where(d => d.NgayKy <= request.ToDt);
                }
                var data = await query.OrderBy(d => d.NgayKy).ToListAsync();
                result.Data = data.MapTo<RevenueResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<PriceQuoteResponse>> TotalPriceQuote (ChartRequest request)
        {
            var result = new BaseResponse<PriceQuoteResponse> { };
            try
            {
                var query = _datacontext.WMS_Quotations.AsQueryable();
                if (request.FromDt.HasValue)
                {
                    query = query.Where(d => d.Date > request.FromDt);
                }
                if (request.ToDt.HasValue)
                {
                    query = query.Where(d => d.Date <= request.ToDt);
                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<PriceQuoteResponse>();
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
