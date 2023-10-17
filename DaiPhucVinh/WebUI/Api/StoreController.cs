using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.FoodList;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using DaiPhucVinh.Shared.Province;
using DaiPhucVinh.Shared.Store;
using DaiPhucVinh.Shared.User;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/store")]
    public class StoreController : BaseApiController
    {
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpPost]
        [Route("TakeAllStore")]
        public async Task<BaseResponse<StoreResponse>> TakeAllStore([FromBody] StoreRequest request) => await _storeService.TakeAllStore(request);

        [HttpPost]
        [Route("SearchStore")]
        public async Task<BaseResponse<StoreResponse>> SearchStore([FromBody] StoreRequest request) => await _storeService.SearchStore(request);

        [HttpPost]
        [Route("TakeAllOrder")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeAllOrder([FromBody] OrderHeaderRequest request) => await _storeService.TakeAllOrder(request);

        [HttpPost]
        [Route("TakeAllDeliveryDriver")]
        public async Task<BaseResponse<DeliveryDriverResponse>> TakeAllDeliveryDriver([FromBody] DeliveryDriverRequest request) => await _storeService.TakeAllDeliveryDriver(request);
        [HttpPost]
        [Route("ApproveOrder")]
        public async Task<BaseResponse<bool>> ApproveOrder([FromBody] OrderHeaderRequest request) => await _storeService.ApproveOrder(request);
        [HttpPost]
        [Route("ApproveDelvery")]
        public async Task<BaseResponse<bool>> ApproveDelvery([FromBody] DeliveryDriverRequest request) => await _storeService.ApproveDelvery(request);

        [HttpPost]
        [Route("CreateNewStore")]
        public async Task<BaseResponse<bool>> CreateNewStore()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                img = httpRequest.Files[0];
            }
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];

                var request = JsonConvert.DeserializeObject<StoreRequest>(jsonRequest);
                return await _storeService.CreateNewStore(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }

        [HttpPost]
        [Route("CreateNewDeliver")]
        public async Task<BaseResponse<bool>> CreateNewDeliver()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                img = httpRequest.Files[0];
            }
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];

                var request = JsonConvert.DeserializeObject<DeliveryDriverRequest>(jsonRequest);
                return await _storeService.CreateNewDeliver(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }

        [HttpPost]
        [Route("DeleteStore")]
        public async Task<BaseResponse<bool>> DeleteStore([FromBody] StoreRequest request) => await _storeService.DeleteStore(request);

        [HttpGet]
        [Route("TakeStoreById")]
        public async Task<BaseResponse<StoreResponse>> TakeStoreById(int Id) => await _storeService.TakeStoreById(Id);
        [HttpGet]
        [Route("TakeDriverById")]
        public async Task<BaseResponse<DeliveryDriverResponse>> TakeDriverById(int Id) => await _storeService.TakeDriverById(Id);
        
        [HttpPost]
        [Route("TakeStoreByCuisineId")]
        public async Task<BaseResponse<StoreResponse>> TakeStoreByCuisineId(FilterStoreByCusineRequest filter) => await _storeService.TakeStoreByCuisineId(filter);
        [HttpGet]
        [Route("TakeCategoryByStoreId")]
        public async Task<BaseResponse<CategoryListResponse>> TakeCategoryByStoreId(int Id) => await _storeService.TakeCategoryByStoreId(Id);
        [HttpGet]
        [Route("TakeFoodListByStoreId")]
        public async Task<BaseResponse<FoodListResponse>> TakeFoodListByStoreId(int Id) => await _storeService.TakeFoodListByStoreId(Id);
        [HttpGet]
        [Route("TakeAllFoodListByStoreId")]
        public async Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(int Id) => await _storeService.TakeAllFoodListByStoreId(Id);
        [HttpPost]
        [Route("ApproveStore")]
        public async Task<BaseResponse<bool>> ApproveStore([FromBody] StoreRequest request) => await _storeService.ApproveStore(request);
        [HttpGet]
        [Route("TakeOrderHeaderByStoreId")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int Id) => await _storeService.TakeOrderHeaderByStoreId(Id);
        [HttpGet]
        [Route("GetListOrderLineDetails")]
        public async Task<BaseResponse<OrderLineReponse>> GetListOrderLineDetails(string Id) => await _storeService.GetListOrderLineDetails(Id);
        [HttpPost]
        [Route("TakeStatisticalByStoreId")]
        public async Task<BaseResponse<StatisticalResponse>> TakeStatisticalByStoreId([FromBody] StatisticalRequest request) => await _storeService.TakeStatisticalByStoreId(request);

        [HttpPost]
        [Route("TakeStoreByUserLogin")]
        public async Task<BaseResponse<StoreResponse>> TakeStoreByUserLogin(FilterStoreByCusineRequest filter) => await _storeService.TakeStoreByUserLogin(filter);
        
        [HttpPost]
        [Route("TakeStoreByCuisineUserLogin")]
        public async Task<BaseResponse<StoreResponse>> TakeStoreByCuisineUserLogin(FilterStoreByCusineRequest filter) => await _storeService.TakeStoreByCuisineUserLogin(filter);
        
        [HttpPost]
        [Route("PostAllFoodListByStoreId")]
        public async Task<BaseResponse<FoodListResponse>> PostAllFoodListByStoreId([FromBody] SimpleUserRequest request) => await _storeService.PostAllFoodListByStoreId(request);

        [HttpPost]
        [Route("TakeAllOrderLineByCustomerId")]
        public async Task<BaseResponse<OrderLineReponse>> TakeAllOrderLineByCustomerId([FromBody] EN_CustomerRequest request) => await _storeService.TakeAllOrderLineByCustomerId(request);
        [HttpPost]
        [Route("RemoveDriver")]
        public async Task<BaseResponse<bool>> RemoveDriverr([FromBody] DeliveryDriverRequest request) => await _storeService.RemoveDriverr(request);

    }
}