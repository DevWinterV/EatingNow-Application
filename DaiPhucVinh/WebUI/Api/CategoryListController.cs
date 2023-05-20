using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.CategoryList;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/categorylist")]
    public class CategoryListController : BaseApiController
    {
        private readonly ICategoryListService _categoryListService;
        public CategoryListController(ICategoryListService categoryListService)
        {
            _categoryListService = categoryListService;
        }
        [HttpPost]
        [Route("CreateCategoryList")]
        public async Task<BaseResponse<bool>> CreateCategoryList([FromBody] CategoryListRequest request) => await _categoryListService.CreateCategoryList(request);

        [HttpPost]
        [Route("UpdateCategoryList")]
        public async Task<BaseResponse<bool>> UpdateCategoryList([FromBody] CategoryListRequest request) => await _categoryListService.UpdateCategoryList(request);

        [HttpPost]
        [Route("DeleteCategoryList")]
        public async Task<BaseResponse<bool>> DeleteCategoryList([FromBody] CategoryListRequest request) => await _categoryListService.DeleteCategoryList(request);
    }
}