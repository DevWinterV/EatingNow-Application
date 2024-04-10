using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Chart_DashBoarb;
using DaiPhucVinh.Shared.Chart;
using DaiPhucVinh.Shared.Common;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [Authorize]
    [RoutePrefix("api/dashboard")]
    public class ChartController : SecureApiController
    {
        private readonly IChartService _chartService;

        public ChartController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpPost]
        [Route("TotalRevenue_Chart")]
        public async Task<BaseResponse<RevenueResponse>> TotalRevenue_Chart([FromBody] ChartRequest request) => await _chartService.TotalRevenue_Chart(request);
        
        [HttpPost]
        [Route("TotalPriceQuote")]
        public async Task<BaseResponse<PriceQuoteResponse>> TotalPriceQuote([FromBody] ChartRequest request) => await _chartService.TotalPriceQuote(request);

        [HttpPost]
        [Route("TotalRevenueStatistics")]
        public async Task<BaseResponse<RevenuestatisticsResponse>> TotalPriceQuote([FromBody] StatisticRequest request) => await _chartService.TotalRevenueStatistics(request);

        [HttpPost]
        [Route("TakeProductStatistics")]
        public async Task<BaseResponse<List<ItemFoodResponse>>> TakeProductStatistics([FromBody] StatisticRequest request) => await _chartService.TakeProductStatistics(request);

    }
}