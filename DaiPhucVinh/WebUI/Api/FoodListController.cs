using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.FoodList;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.FoodList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/foodlist")]
    public class FoodListController : BaseApiController
    {
        private readonly IFoodListService _foodListService;
        public FoodListController(IFoodListService foodListService)
        {
            _foodListService = foodListService;
        }
        [HttpPost]
        [Route("CreateFoodItem")]
        public async Task<BaseResponse<bool>> CreateFoodItem()
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

                var request = JsonConvert.DeserializeObject<FoodListRequest>(jsonRequest);
                return await _foodListService.CreateFoodItem(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("UpdateFoodList")]
        public async Task<BaseResponse<bool>> UpdateFoodList()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                img = httpRequest.Files[0];

                if (httpRequest.Form.Count > 0)
                {
                    var jsonRequest = httpRequest.Form[0];

                    var request = JsonConvert.DeserializeObject<FoodListRequest>(jsonRequest);
                    return await _foodListService.UpdateFoodListHaveImage(request, img);
                }
                else return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "File not found!"
                };
            }
            else
            {
                if (httpRequest.Form.Count > 0)
                {
                    var jsonRequest = httpRequest.Form[0];

                    var request = JsonConvert.DeserializeObject<FoodListRequest>(jsonRequest);
                    return await _foodListService.UpdateFoodListNotImage(request);
                }
                else return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "File not found!"
                };
            }
        }

        [HttpPost]
        [Route("DeleteFoodList")]
        public async Task<BaseResponse<bool>> DeleteFoodList([FromBody] FoodListRequest request) => await _foodListService.DeleteFoodList(request);
        [HttpGet]
        [Route("TakeFoodListById")]
        public async Task<BaseResponse<FoodListResponse>> TakeFoodListById(int FoodListId) => await _foodListService.TakeFoodListById(FoodListId);
        [HttpPost]
        [Route("ChangeIsNoiBatFoodList")]
        public async Task<BaseResponse<bool>> ChangeIsNoiBatFoodList([FromBody] FoodListRequest request) => await _foodListService.ChangeIsNoiBatFoodList(request);
        [HttpPost]
        [Route("ChangeIsNewFoodList")]
        public async Task<BaseResponse<bool>> ChangeIsNewFoodList([FromBody] FoodListRequest request) => await _foodListService.ChangeIsNewFoodList(request);
    }
}