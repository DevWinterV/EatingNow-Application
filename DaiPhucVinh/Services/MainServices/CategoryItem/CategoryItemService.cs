using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Shared.CategoryItem;
using DaiPhucVinh.Shared.Common;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.CategoryItem
{
    public interface ICategoryItemService
    {
        Task<BaseResponse<CategoryItemResponse>> TakeAllCategoryItem(CategoryItemRequest request);

        //Task<BaseResponse<ProductDto>> TakeGroupCategoryItemByCode(ProductRequest request);

        #region webadmin
        Task<BaseResponse<CategoryItemResponse>> TakeAllItemCategory(CategoryItemRequest request);
        Task<BaseResponse<bool>> CreateItemCategory(CategoryItemRequest request);
        Task<BaseResponse<bool>> UpdateItemCategory(CategoryItemRequest request);
        Task<BaseResponse<bool>> RemoveItemCategory(CategoryItemRequest request);
        #endregion
    }
    public class CategoryItemService : ICategoryItemService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        public CategoryItemService(DataContext datacontext, ILogService logService, ICommonService commonService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
        }
        #region nhóm sản phẩm webadmin
        public async Task<BaseResponse<CategoryItemResponse>> TakeAllItemCategory(CategoryItemRequest request)
        {
            var result = new BaseResponse<CategoryItemResponse> { };
            try
            {
                var query = _datacontext.WMS_ItemCategories.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<CategoryItemResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateItemCategory(CategoryItemRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var categoryitem = new WMS_ItemCategory
                {
                    Code = await _commonService.AutoGencode(nameof(WMS_ItemCategory)),
                    Name = request.Name,
                    CreationDate = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    LastUpdateDate = DateTime.Now,
                    LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                };
                _datacontext.WMS_ItemCategories.Add(categoryitem);
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
        public async Task<BaseResponse<bool>> RemoveItemCategory(CategoryItemRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var itemcategory = await _datacontext.WMS_ItemCategories.FirstOrDefaultAsync(x => x.Id == request.Id);
                _datacontext.WMS_ItemCategories.Remove(itemcategory);
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
        public async Task<BaseResponse<bool>> UpdateItemCategory(CategoryItemRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = await _datacontext.WMS_ItemCategories.FirstOrDefaultAsync(x => x.Id == request.Id);
                query.Name = request.Name;
                query.LastUpdateDate = DateTime.Now;
                query.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        #endregion
        public async Task<BaseResponse<CategoryItemResponse>> TakeAllCategoryItem(CategoryItemRequest request)
        {
            var result = new BaseResponse<CategoryItemResponse> { };
            try
            {
                var query = _datacontext.WMS_ItemCategories.AsQueryable();
                var queryItem_Group = _datacontext.WMS_ItemGroups.AsQueryable();
                var queryImageRecord = _datacontext.ImageRecords.AsQueryable();

                if (request.Term.HasValue() && request.Term != "")
                {
                    query = query.Where(d => d.Name.Contains(request.Term));
                }
                var data = await query.OrderBy(d => d.Id).ToListAsync();
                var mapdata = data.MapTo<CategoryItemResponse>();

                result.Data = mapdata;
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        //public async Task<BaseResponse<ProductDto>> TakeGroupCategoryItemByCode(ProductRequest request)
        //{
        //    var result = new BaseResponse<ProductDto> { };
        //    try
        //    {
        //        var query = _datacontext.WMS_Items.AsQueryable();
        //        var queryImageRecord = _datacontext.ImageRecords.AsQueryable();

        //        if (!string.IsNullOrEmpty(request.ItemCode))
        //        {
        //            query = query.Where(d => d.ItemCategoryCode == request.ItemCode && d.IsDelete.HasValue && !d.IsDelete.Value);
        //        }

        //        if (request.Term.HasValue() && request.Term != "")
        //        {
        //            query = query.Where(d => d.Name.Contains(request.Term));
        //        }
        //        var data = await query.OrderByDescending(d => d.Id).ToListAsync();
        //        var mapdata = data.MapTo<ProductDto>();

        //        foreach (var item in mapdata)
        //        {
        //            List<ImageRecord> lstImg = new List<ImageRecord>();
        //            if (item.ImageRecordId != null)
        //            {
        //                var listImageId = item.ImageRecordId.Split(',').Where(x => int.TryParse(x, out _))
        //                                                            .Select(int.Parse)
        //                                                            .ToList();
        //                foreach (var id in listImageId)
        //                {
        //                    var dataId = queryImageRecord.FirstOrDefault(x => x.Id == id);
        //                    lstImg.Add(dataId);
        //                }
        //            }
        //            item.ListImage = lstImg;
        //        }

        //        result.Data = mapdata;
        //        result.Success = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.ToString();
        //    }
        //    return result;
        //}
    }
}
