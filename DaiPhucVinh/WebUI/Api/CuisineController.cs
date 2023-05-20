using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Cuisine;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Cuisine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/cuisine")]
    public class CuisineController : BaseApiController
    {
        private readonly ICuisineService _cuisineService;
        public CuisineController(ICuisineService cuisineService)
        {
            _cuisineService = cuisineService;
        }

        [HttpPost]
        [Route("TakeAllCuisine")]
        public async Task<BaseResponse<CuisineResponse>> TakeAllCuisine([FromBody] CuisineRequest request) => await _cuisineService.TakeAllCuisine(request);

        [HttpPost]
        [Route("CreateNewCuisine")]
        public async Task<BaseResponse<bool>> CreateNewCuisine()
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

                var request = JsonConvert.DeserializeObject<CuisineRequest>(jsonRequest);
                return await _cuisineService.CreateNewCuisine(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("UpdateNewCuisine")]
        public async Task<BaseResponse<bool>> UpdateNewCuisine()
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

                    var request = JsonConvert.DeserializeObject<CuisineRequest>(jsonRequest);
                    return await _cuisineService.UpdateNewCuisineHaveImage(request, img);
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

                    var request = JsonConvert.DeserializeObject<CuisineRequest>(jsonRequest);
                    return await _cuisineService.UpdateNewCuisineNotImage(request);
                }
                else return new BaseResponse<bool>
                {
                    Success = false,
                    Message = "File not found!"
                };
            }
        }

        [HttpPost]
        [Route("DeleteCuisine")]
        public async Task<BaseResponse<bool>> DeleteCuisine([FromBody] CuisineRequest request) => await _cuisineService.DeleteCuisine(request);

        [HttpPost]
        [Route("SearchCuisine")]
        public async Task<BaseResponse<CuisineResponse>> SearchCuisine(string cuisineName) => await _cuisineService.SearchCuisine(cuisineName);

        [HttpGet]
        [Route("TakeCuisineById")]
        public async Task<BaseResponse<CuisineResponse>> TakeCuisineById(int Id) => await _cuisineService.TakeCuisineById(Id);
    }
}