using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.District;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.District;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/district")]
    public class DistrictController : BaseApiController
    {
        private readonly IDistrictService _districtService;
        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }
        [HttpPost]
        [Route("TakeAllDistrict")]
        public async Task<BaseResponse<DistrictResponse>> TakeAllDistrict([FromBody] DistrictRequest request) => await _districtService.TakeAllDistrict(request);
        [Authorize]

        [HttpPost]
        [Route("CreateNewDistrict")]
        public async Task<BaseResponse<bool>> CreateNewDistrict([FromBody] DistrictRequest request) => await _districtService.CreateNewDistrict(request);
        [Authorize]

        [HttpPost]
        [Route("UpdateNewDistrict")]
        public async Task<BaseResponse<bool>> UpdateNewDistrict([FromBody] DistrictRequest request) => await _districtService.UpdateNewDistrict(request);
        [Authorize]

        [HttpPost]
        [Route("DeleteDistrict")]
        public async Task<BaseResponse<bool>> DeleteDistrict([FromBody] DistrictRequest request) => await _districtService.DeleteDistrict(request);
        [Authorize]

        [HttpPost]
        [Route("SearchDistrict")]
        public async Task<BaseResponse<DistrictResponse>> SearchDistrict(string cuisineName) => await _districtService.SearchDistrict(cuisineName);
        [Authorize]

        [HttpGet]
        [Route("TakeDistrictById")]
        public async Task<BaseResponse<DistrictResponse>> TakeDistrictById(int Id) => await _districtService.TakeDistrictById(Id);
    }
}