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

        [HttpPost]
        [Route("CreateNewDistrict")]
        public async Task<BaseResponse<bool>> CreateNewDistrict([FromBody] DistrictRequest request) => await _districtService.CreateNewDistrict(request);

        [HttpPost]
        [Route("UpdateNewDistrict")]
        public async Task<BaseResponse<bool>> UpdateNewDistrict([FromBody] DistrictRequest request) => await _districtService.UpdateNewDistrict(request);

        [HttpPost]
        [Route("DeleteDistrict")]
        public async Task<BaseResponse<bool>> DeleteDistrict([FromBody] DistrictRequest request) => await _districtService.DeleteDistrict(request);

        [HttpPost]
        [Route("SearchDistrict")]
        public async Task<BaseResponse<DistrictResponse>> SearchDistrict(string cuisineName) => await _districtService.SearchDistrict(cuisineName);

        [HttpGet]
        [Route("TakeDistrictById")]
        public async Task<BaseResponse<DistrictResponse>> TakeDistrictById(int Id) => await _districtService.TakeDistrictById(Id);
    }
}