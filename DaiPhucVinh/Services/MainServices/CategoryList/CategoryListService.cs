using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.CategoryList
{
    public interface ICategoryListService
    {
        Task<BaseResponse<bool>> CreateCategoryList(CategoryListRequest request);
        Task<BaseResponse<bool>> UpdateCategoryList(CategoryListRequest request);
        Task<BaseResponse<bool>> DeleteCategoryList(CategoryListRequest request);
    }
    public class CategoryListService : ICategoryListService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public CategoryListService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        
        public async Task<BaseResponse<bool>> CreateCategoryList(CategoryListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {

                var categoryList = new EN_CategoryList
                {
                    CategoryName = request.CategoryName,
                    UserId = request.UserId,
                    Status = request.Status,
                };
                _datacontext.EN_CategoryList.Add(categoryList);
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> UpdateCategoryList(CategoryListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.CategoryId > 0)
                {
                    var categoryList = await _datacontext.EN_CategoryList.FindAsync(request.CategoryId);
                    {
                        categoryList.CategoryName = request.CategoryName;
                    };
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> DeleteCategoryList(CategoryListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var categoryList = await _datacontext.EN_CategoryList.FirstOrDefaultAsync(x => x.CategoryId == request.CategoryId);
                _datacontext.EN_CategoryList.Remove(categoryList);
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
    }
}
