using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Chart_DashBoarb;
using DaiPhucVinh.Shared.Chart;
using DaiPhucVinh.Shared.Common;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
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

    }
}