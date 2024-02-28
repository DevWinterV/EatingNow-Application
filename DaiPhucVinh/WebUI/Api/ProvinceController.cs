using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Province;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/province")]
    public class ProvinceController : BaseApiController
    {
        private readonly IProvinceService _provinceService;
        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }
        [HttpPost]
        [Route("TakeAllProvince")]
        public async Task<BaseResponse<ProvinceResponse>> TakeAllProvince([FromBody] ProvinceRequest request) => await _provinceService.TakeAllProvince(request);
        [Authorize]
        [HttpPost]
        [Route("CreateNewProvince")]
        public async Task<BaseResponse<bool>> CreateNewCuisine([FromBody] ProvinceRequest request) => await _provinceService.CreateNewProvince(request);
        [Authorize]
        [HttpPost]
        [Route("UpdateNewProvince")]
        public async Task<BaseResponse<bool>> UpdateNewCuisine([FromBody] ProvinceRequest request) => await _provinceService.UpdateNewProvince(request);
        [Authorize]
        [HttpPost]
        [Route("DeleteProvince")]
        public async Task<BaseResponse<bool>> DeleteCuisine([FromBody] ProvinceRequest request) => await _provinceService.DeleteProvince(request);
        [Authorize]
        [HttpPost]
        [Route("SearchProvince")]
        public async Task<BaseResponse<ProvinceResponse>> SearchCuisine(string provinceName) => await _provinceService.SearchProvince(provinceName);
        [Authorize]
        [HttpGet]
        [Route("TakeProvinceById")]
        public async Task<BaseResponse<ProvinceResponse>> TakePostById(int Id) => await _provinceService.TakeProvinceById(Id);
    }
}