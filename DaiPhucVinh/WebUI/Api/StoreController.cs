using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.FoodList;
using DaiPhucVinh.Shared.OrderHeader;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using DaiPhucVinh.Shared.Province;
using DaiPhucVinh.Shared.Store;
using DaiPhucVinh.Shared.User;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

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

        [Authorize]
        [HttpPost]
        [Route("TakeAllStore")]
        public async Task<BaseResponse<StoreResponse>> TakeAllStore([FromBody] StoreRequest request) => await _storeService.TakeAllStore(request);
        [Authorize]
        [HttpPost]
        [Route("SearchStore")]
        public async Task<BaseResponse<StoreResponse>> SearchStore([FromBody] StoreRequest request) => await _storeService.SearchStore(request);
        [Authorize]
        [HttpPost]
        [Route("TakeAllOrder")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeAllOrder([FromBody] OrderHeaderRequest request) => await _storeService.TakeAllOrder(request);
        [Authorize]
        [HttpPost]
        [Route("TakeAllDeliveryDriver")]
        public async Task<BaseResponse<DeliveryDriverResponse>> TakeAllDeliveryDriver([FromBody] DeliveryDriverRequest request) => await _storeService.TakeAllDeliveryDriver(request);
        
        [HttpPost]
        [Route("ApproveOrder")]
        public async Task<BaseResponse<bool>> ApproveOrder([FromBody] OrderHeaderRequest request) => await _storeService.ApproveOrder(request);
        [Authorize]
        [HttpPost]
        [Route("ApproveDelvery")]
        public async Task<BaseResponse<bool>> ApproveDelvery([FromBody] DeliveryDriverRequest request) => await _storeService.ApproveDelvery(request);
       
        [Authorize]
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

                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new Conversion.TimeSpanConverter() }
                };

                //var storeTime = JsonConvert.DeserializeObject<StoreTime>(jsonString, settings);

                //var request = JsonConvert.DeserializeObject<StoreRequest>(jsonRequest, settings);
                var request = JsonConvert.DeserializeObject<StoreRequest>(jsonRequest);
                return await _storeService.CreateNewStore(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }


        [Authorize]
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
        [Authorize]
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
        [HttpPost]
        [Route("TakeAllFoodListByStoreId")]
        public async Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(FoodListFillterRequest request) => await _storeService.TakeAllFoodListByStoreId(request);
        [Authorize]
        [HttpPost]
        [Route("ApproveStore")]
        public async Task<BaseResponse<bool>> ApproveStore([FromBody] StoreRequest request) => await _storeService.ApproveStore(request);

        
        [HttpPost]
        [Route("TakeOrderHeaderByStoreId")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(OrderHeaderFillterRequest request) => await _storeService.TakeOrderHeaderByStoreId(request);
       
        
        [HttpGet]
        [Route("GetListOrderLineDetails")]
        public async Task<BaseResponse<OrderLineReponse>> GetListOrderLineDetails(string Id) => await _storeService.GetListOrderLineDetails(Id);
        [HttpPost]
        [Route("TakeStatisticalByStoreId")]
        public async Task<BaseResponse<StatisticalResponse>> TakeStatisticalByStoreId([FromBody] StatisticalRequest request) => await _storeService.TakeStatisticalByStoreId(request);

        [HttpPost]
        [Route("TakeProductTrackingByStoreId")]
        public async Task<BaseResponse<FoodListResponse>> TakeProductTrackingByStoreId([FromBody] StatisticalRequest request) => await _storeService.TakeProductTrackingByStoreId(request);

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

        [Authorize]
        [HttpPost]
        [Route("RemoveDriver")]
        public async Task<BaseResponse<bool>> RemoveDriverr([FromBody] DeliveryDriverRequest request) => await _storeService.RemoveDriverr(request);

        [HttpPost]
        [Route("TakeStoreLocation")]
        public async Task<BaseResponse<StoreResponse>> TakeStoreLocation([FromBody] StoreRequest request) => await _storeService.TakeStoreLocation(request);


        [HttpGet]
        [Route("TakeLitsFoodSold")]
        public async Task<BaseResponse<ListOfProductSold>> TakeLitsFoodSold(int UserId) => await _storeService.TakeLitsFoodSold(UserId);

        [HttpGet]
        [Route("TakeStoreInfoByStoreManager")]
        public async Task<BaseResponse<StoreInfoTakeByStoreManagerResponse>> TakeStoreInfoByStoreManager(int UserId) => await _storeService.TakeStoreInfoByStoreManager(UserId);
        //[Authorize]
       
        
        [HttpPut]
        [Route("UpdateStoreInfoByStoreManager")]
        public async Task<BaseResponse<bool>> UpdateStoreInfoByStoreManager()
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

                var request = JsonConvert.DeserializeObject<StoreInfoTakeByStoreManagerRequest>(jsonRequest);
                return await _storeService.UpdateStoreInfoByStoreManager(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }

    }
}