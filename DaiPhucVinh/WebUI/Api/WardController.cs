using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.District;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.District;
using DaiPhucVinh.Shared.Ward;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/ward")]
    public class WardController : BaseApiController
    {
        private readonly IWardService _wardService;
        public WardController(IWardService wardService)
        {
            _wardService = wardService;
        }
        [HttpPost]
        [Route("TakeAllWard")]
        public async Task<BaseResponse<WardResponse>> TakeAllProvince([FromBody] WardRequest request) => await _wardService.TakeAllWard(request);
        [Authorize]
        [HttpPost]
        [Route("CreateNewWard")]
        public async Task<BaseResponse<bool>> CreateNewCuisine([FromBody] WardRequest request) => await _wardService.CreateNewWard(request);
        [Authorize]
        [HttpPost]
        [Route("UpdateNewWard")]
        public async Task<BaseResponse<bool>> UpdateNewCuisine([FromBody] WardRequest request) => await _wardService.UpdateNewWard(request);
        [Authorize]
        [HttpPost]
        [Route("DeleteWard")]
        public async Task<BaseResponse<bool>> DeleteCuisine([FromBody] WardRequest request) => await _wardService.DeleteWard(request);
        [Authorize]
        [HttpPost]
        [Route("SearchWard")]
        public async Task<BaseResponse<WardResponse>> SearchCuisine(string cuisineName) => await _wardService.SearchWard(cuisineName);
        [Authorize]
        [HttpGet]
        [Route("TakeWardById")]
        public async Task<BaseResponse<WardResponse>> TakePostById(int Id) => await _wardService.TakeWardById(Id);
    }
}