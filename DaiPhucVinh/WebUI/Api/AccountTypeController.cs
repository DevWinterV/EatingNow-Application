using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.AccountType;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Province;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [Authorize]
    [RoutePrefix("api/accounttype")]
    public class AccountTypeController : BaseApiController
    {
        private readonly IAccountTypeService _accountTypeService;
        public AccountTypeController(IAccountTypeService provinceService)
        {
            _accountTypeService = provinceService;
        }
        [HttpPost]
        [Route("TakeAllAccountType")]
        public async Task<BaseResponse<AccountTypeResponse>> TakeAllAccountType([FromBody] AccountTypeRequest request) => await _accountTypeService.TakeAllAccountType(request);

        [HttpPost]
        [Route("CreateNewAccountType")]
        public async Task<BaseResponse<bool>> CreateNewCuisine([FromBody] AccountTypeRequest request) => await _accountTypeService.CreateNewAccountType(request);

        [HttpPost]
        [Route("UpdateNewAccountType")]
        public async Task<BaseResponse<bool>> UpdateNewCuisine([FromBody] AccountTypeRequest request) => await _accountTypeService.UpdateNewAccountType(request);

        [HttpPost]
        [Route("DeleteAccountType")]
        public async Task<BaseResponse<bool>> DeleteCuisine([FromBody] AccountTypeRequest request) => await _accountTypeService.DeleteAccountType(request);

        [HttpPost]
        [Route("SearchAccountType")]
        public async Task<BaseResponse<AccountTypeResponse>> SearchCuisine(string provinceName) => await _accountTypeService.SearchAccountType(provinceName);

        [HttpGet]
        [Route("TakeAccountTypeById")]
        public async Task<BaseResponse<AccountTypeResponse>> TakePostById(int Id) => await _accountTypeService.TakeAccountTypeById(Id);
    }
}