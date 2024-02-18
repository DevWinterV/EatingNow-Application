using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.FoodList;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using DaiPhucVinh.Shared.Province;
using DaiPhucVinh.Shared.Store;
using DaiPhucVinh.Shared.User;
using Falcon.Core.Domain.Messages;
using Falcon.Web.Core.Log;
using Falcon.Web.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DotNetty.Common.Utilities;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.Customer;
using Microsoft.AspNet.SignalR;
using DaiPhucVinh.Services.MainServices.Hubs;
using DaiPhucVinh.Services.MainServices.EN_CustomerService;
using System.Xml.Linq;
using DaiPhucVinh.Shared.OrderHeader;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace DaiPhucVinh.Services.MainServices.Province
{
    public interface IStoreService
    {
        Task<BaseResponse<StoreResponse>> TakeAllStore(StoreRequest request);
        Task<BaseResponse<StoreResponse>> SearchStore(StoreRequest request);
        Task<BaseResponse<DeliveryDriverResponse>> TakeAllDeliveryDriver(DeliveryDriverRequest request);
        Task<BaseResponse<OrderHeaderResponse>> TakeAllOrder(OrderHeaderRequest request);

        Task<BaseResponse<bool>> CreateNewStore(StoreRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> DeleteStore(StoreRequest request);
        Task<BaseResponse<StoreResponse>> TakeStoreById(int Id);
        Task<BaseResponse<StoreResponse>> TakeStoreByCuisineId(FilterStoreByCusineRequest filter);
        Task<BaseResponse<CategoryListResponse>> TakeCategoryByStoreId(int Id);
        Task<BaseResponse<FoodListResponse>> TakeFoodListByStoreId(int Id);
        Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(FoodListFillterRequest request);
        Task<BaseResponse<bool>> ApproveStore(StoreRequest request);
        Task<BaseResponse<bool>> ApproveDelvery(DeliveryDriverRequest request);

        Task<BaseResponse<bool>> ApproveOrder(OrderHeaderRequest request);

        Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(OrderHeaderFillterRequest request);
        Task<BaseResponse<OrderLineReponse>> GetListOrderLineDetails(string Id);
        Task<BaseResponse<StatisticalResponse>> TakeStatisticalByStoreId(StatisticalRequest request);
        Task<BaseResponse<StoreResponse>> TakeStoreByUserLogin(FilterStoreByCusineRequest filter);
        Task<BaseResponse<StoreResponse>> TakeStoreByCuisineUserLogin(FilterStoreByCusineRequest filter);
        Task<BaseResponse<FoodListResponse>> PostAllFoodListByStoreId(SimpleUserRequest request);
        Task<BaseResponse<OrderLineReponse>> TakeAllOrderLineByCustomerId(EN_CustomerRequest request);
        Task<BaseResponse<bool>> CreateNewDeliver(DeliveryDriverRequest request, HttpPostedFile file);
        Task<BaseResponse<DeliveryDriverResponse>> TakeDriverById(int Id);
        Task<BaseResponse<bool>> RemoveDriverr(DeliveryDriverRequest request);
        Task<BaseResponse<StoreResponse>> TakeStoreLocation(StoreRequest request);
        Task<BaseResponse<ListOfProductSold>> TakeLitsFoodSold(int UserId);

    }
    public class StoreService : IStoreService
    {
        Random rand = new Random();
        const string SubPath = "\\uploads\\DaiPhucVinh\\Image";
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;

        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public StoreService(DataContext datacontext, ILogService logService, ICommonService commonService, ILogger logger, ISettingService settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
            _settingService = settingService;
            _logger = logger;
        }
        /// <summary>
        /// Lấy tất cả các tài xế trên hệ thống
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<DeliveryDriverResponse>> TakeAllDeliveryDriver(DeliveryDriverRequest request)
        {
            var result = new BaseResponse<DeliveryDriverResponse> { };
            try
            {
                if(request.Term == null)
                {
                    var query = _datacontext.EN_DeliveryDriver.AsQueryable();
                    if (request.ProvinceId > 0)
                    {
                        query = query.Where(x => x.ProvinceId.Equals(request.ProvinceId)).OrderBy(d => d.CompleteName);
                        result.DataCount = await query.CountAsync();
                        if (request.PageSize != 0)
                        {
                            query = query.OrderBy(d => d.DeliveryDriverId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        }
                        var data = await query.ToListAsync();
                        result.Data = data.MapTo<DeliveryDriverResponse>();
                    }
                    else {
                        query = query.OrderBy(d => d.CompleteName);
                        result.DataCount = await query.CountAsync();
                        if (request.PageSize != 0)
                        {
                            query = query.OrderBy(d => d.DeliveryDriverId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        }
                        var data = await query.ToListAsync();
                        result.Data = data.MapTo<DeliveryDriverResponse>();
                    }
                 
                    result.Success = true;
                }
                else
                {
                    var query = _datacontext.EN_DeliveryDriver
                        .Where(x=> x.CompleteName.Contains(request.Term)
                        || x.Phone.Contains(request.Term)
                        || x.Email.Contains(request.Term));
                    query = query.OrderBy(d => d.CompleteName);
                    result.DataCount = await query.CountAsync();
                    if (request.PageSize != 0)
                    {
                        query = query.OrderBy(d => d.DeliveryDriverId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    }
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<DeliveryDriverResponse>();
                    result.Success = true;
                }
               
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<StoreResponse>> TakeStoreByCuisineUserLogin(FilterStoreByCusineRequest filter)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var query = _datacontext.EN_Store.Where(x => x.Status && x.CuisineId == filter.CuisineId).AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = FindNearestStores(data.MapTo<StoreResponse>(), filter.latitude, filter.longitude, filter.Count).MapTo<StoreResponse>();
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<StoreResponse>> TakeStoreByUserLogin(FilterStoreByCusineRequest filter)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var query = _datacontext.EN_Store.Where(x => x.Status).AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = FindNearestStores(data.MapTo<StoreResponse>(), filter.latitude, filter.longitude, filter.Count).MapTo<StoreResponse>();
                result.Data = (IList<StoreResponse>)resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }


        public async Task<BaseResponse<StoreResponse>> TakeAllStore(StoreRequest request)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                if(request.ProvinceId == null || request.ProvinceId == 0)
                {
                    if (request.ItemCategoryCode == 0)
                    {
                        var query = _datacontext.EN_Store.AsQueryable();
                        query = query.OrderBy(d => d.FullName);
                        result.DataCount = await query.CountAsync();
                        if (request.PageSize != 0)
                        {
                            query = query.OrderBy(d => d.UserId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        }
                        var data = await query.ToListAsync();
                        result.Data = data.MapTo<StoreResponse>();
                        result.Success = true;
                    }
                    else
                    {
                        var query = _datacontext.EN_Store.Where(x => x.CuisineId == request.ItemCategoryCode).AsQueryable();
                        query = query.OrderBy(d => d.FullName);
                        result.DataCount = await query.CountAsync();
                        if (request.PageSize != 0)
                        {
                            query = query.OrderBy(d => d.UserId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        }
                        var data = await query.ToListAsync();
                        result.Data = data.MapTo<StoreResponse>();
                        result.Success = true;
                    }
                }
                else
                {
                    var query = _datacontext.EN_Store.Where(x => x.ProvinceId == request.ProvinceId).AsQueryable();
                    query = query.OrderBy(d => d.FullName);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<StoreResponse>();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        /// <summary>
        /// Chức năng tìm kiếm cửa hàng theo tên
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<StoreResponse>> SearchStore(StoreRequest request)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                if (request.Term == "")
                {
                    var query = _datacontext.EN_Store.AsQueryable();
                    query = query.OrderBy(d => d.FullName);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<StoreResponse>();
                    result.Success = true;
                }
                else
                {
                    var query = _datacontext.EN_Store.Where(x => x.FullName.Contains(request.Term ) 
                    || x.Email.Contains(request.Term) || x.OwnerName.Contains(request.Term) 
                    || x.Address.Contains(request.Term)
                    || x.Phone.Contains(request.Term)
                    || x.Description.Contains(request.Term)).AsQueryable();
                    query = query.OrderBy(d => d.FullName);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<StoreResponse>();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<CategoryListResponse>> TakeCategoryByStoreId(int storeId)
        {
            var result = new BaseResponse<CategoryListResponse>();

            try
            {
                // Check if there are categories associated with the given store ID
                var storeExists = await _datacontext.EN_Store.AnyAsync(s => s.UserId == storeId);

                if (storeExists)
                {
                    var query = _datacontext.EN_CategoryList.Where(d => d.Store.UserId == storeId);

                    result.DataCount = await query.CountAsync();

                    if (result.DataCount > 0)
                    {
                        var data = await query.ToListAsync();
                        var resultList = data.MapTo<CategoryListResponse>().ToList();
                        result.Data = resultList;
                    }
                    else
                    {
                        // No categories found for the store, map store information instead
                        var store = await _datacontext.EN_Store.FirstOrDefaultAsync(x => x.UserId.Equals(storeId));
                        result.DataCount = 1;
                        result.Data = new List<CategoryListResponse> { store.MapTo<CategoryListResponse>() };
                    }

                    result.Success = true;
                }
                else
                {
                    // Handle the case where the store is not found
                    result.DataCount = 0;
                    result.Data = null;
                    result.Success = false;
                    result.Message = "Store not found.";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }

            return result;
        }


        public async Task<BaseResponse<FoodListResponse>> TakeFoodListByStoreId(int Id)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var newfoodlist = new List<FoodList_Store>();
                var query = _datacontext.EN_FoodList.AsQueryable();
                query = query.Where(d => d.Category.CategoryId == Id && d.Status == true);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var foodlistfirst = data[0];
                var store = await _datacontext.EN_Store.FirstOrDefaultAsync(x => x.UserId == foodlistfirst.UserId);
                foreach (var item in data)
                {
                    newfoodlist.Add(
                        new FoodList_Store
                        {
                            foodItem = item,
                            Storeitem = store
                        });
                }
                var resultList = newfoodlist.MapTo<FoodListResponse>();
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }



        public async Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(FoodListFillterRequest request)
        {
            var newfoodlist = new List<FoodList_Store>();
            var store = await _datacontext.EN_Store.FirstOrDefaultAsync(x => x.UserId.Equals(request.Id));
            DateTime currentDate = DateTime.Now;
            DateTime oneMonthFromNow = DateTime.Now.AddMonths(1);
            var result = new BaseResponse<FoodListResponse> { };
            try
            {   
                var query = _datacontext.EN_FoodList.AsQueryable();
                query = query.Where(d => d.UserId == request.Id && d.Status == true);
                query = query.Where(d => d.FoodName.Contains(request.keyWord) || d.Description.Contains(request.keyWord) );
                // Có kiểm soát số lượng tồn
                if (request.Qtycontrolled != 2)
                {
                    query = query.Where(x => x.Qtycontrolled.Equals(request.Qtycontrolled == 1 ?  true : false));
                }
                if (request.ExpiryDate != 2)
                {
                    query = query.Where(x => request.ExpiryDate == 1 ? x.ExpiryDate.HasValue : !x.ExpiryDate.HasValue);
                }
                //Còn hạn sử dụng dưới 1 tháng 
                if (request.TimeExpiryDate != 2)
                {
                    query = query.Where(x => x.ExpiryDate != null && x.ExpiryDate >= currentDate && x.ExpiryDate <= oneMonthFromNow);
                }

                /* Có kiểm soát số lượng hay không 
                if (request.QuantitySupplied != 2)
                {
                    query = query.Where(x => x.QtySuppliedcontrolled.Equals(request.QuantitySupplied));
                }*/
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                foreach (var item in data)
                {
                    newfoodlist.Add(
                        new FoodList_Store
                        {
                            foodItem = item,
                            Storeitem = store
                        });
                }
                var resultList = newfoodlist.MapTo<FoodListResponse>();
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<FoodListResponse>> PostAllFoodListByStoreId(SimpleUserRequest request)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                if (request.Id == null) return result;
                var query = _datacontext.EN_FoodList.AsQueryable();
                query = query.Where(d => d.UserId == request.Id);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = data.MapTo<FoodListResponse>();
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<StoreResponse>> TakeStoreById(int Id)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var data = await _datacontext.EN_Store.FindAsync(Id);
                result.Item = data.MapTo<StoreResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<DeliveryDriverResponse>> TakeDriverById(int Id)
        {
            var result = new BaseResponse<DeliveryDriverResponse> { };
            try
            {
                var data = await _datacontext.EN_DeliveryDriver.FindAsync(Id);
                result.Item = data.MapTo<DeliveryDriverResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        /// <summary>
        /// Thêm mới và cập nhật cửa hàng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateNewStore(StoreRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var relativePath = "";
                    if (file != null)
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] pictureBinary = ms.GetBuffer();
                        string storeName = "DaiPhucVinh\\image";
                        var storageFolder = $@"\uploads\{storeName}";
                        if (!Directory.Exists(LocalMapPath(storageFolder)))
                            Directory.CreateDirectory(LocalMapPath(storageFolder));

                        string fileName = Path.GetFileName(file.FileName);

                        string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}" + "-" + $"{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
                        File.WriteAllBytes(storageFolderPath, pictureBinary);
                        relativePath = Path.Combine(storageFolder, newFileName);
                    }

                    if (request.UserId == 0)
                    {
                        if (request.Description == "")
                        {
                            request.Description = null;
                        }
                        var province = new EN_Store
                        {
                            FullName = request.FullName,
                            OpenTime = request.OpenTime,
                            ProvinceId = request.ProvinceId,
                            Email = request.Email,
                            Address = request.Address,
                            OwnerName = request.OwnerName,
                            CuisineId = request.CuisineId,
                            Phone = request.Phone,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            AbsoluteImage = HostAddress + GenAbsolutePath(relativePath),
                            Description = request.Description,
                            Status = false,

                    };
                        result.Message = "Add Success";
                        _datacontext.EN_Store.Add(province);
                    }
                    else
                    {
                        var storeUpdate = _datacontext.EN_Store.Where(x => x.UserId == request.UserId).FirstOrDefault();
                        if (storeUpdate != null)
                        {
                            storeUpdate.FullName = request.FullName;
                            storeUpdate.Email = request.Email;
                            storeUpdate.Address = request.Address;
                            storeUpdate.OwnerName = request.OwnerName;
                            storeUpdate.CuisineId = request.CuisineId;
                            storeUpdate.ProvinceId= request.ProvinceId;
                            storeUpdate.Phone = request.Phone;
                            storeUpdate.Latitude = request.Latitude;
                            storeUpdate.Longitude = request.Longitude;
                            if (storeUpdate.AbsoluteImage != request.AbsoluteImage)
                            {
                                storeUpdate.AbsoluteImage = HostAddress + GenAbsolutePath(relativePath);
                            }
                            storeUpdate.Description = request.Description;
                            storeUpdate.OpenTime = request.OpenTime;
                            storeUpdate.Status = request.Status;
                            result.Message = "Update Success";
                        }
                    }

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

        /// <summary>
        /// Thêm mới và cập nhật tài xế
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> CreateNewDeliver(DeliveryDriverRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var relativePath = "";
                    if (file != null)
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] pictureBinary = ms.GetBuffer();
                        string storeName = "DaiPhucVinh\\image";
                        var storageFolder = $@"\uploads\{storeName}";
                        if (!Directory.Exists(LocalMapPath(storageFolder)))
                            Directory.CreateDirectory(LocalMapPath(storageFolder));

                        string fileName = Path.GetFileName(file.FileName);

                        string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}" + "-" + $"{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
                        File.WriteAllBytes(storageFolderPath, pictureBinary);
                        relativePath = Path.Combine(storageFolder, newFileName);
                    }

                    if (request.DeliveryDriverId == 0)
                    {
                        var province = new EN_DeliveryDiver
                        {
                            CompleteName = request.CompleteName,
                            ProvinceId = request.ProvinceId,
                            DistrictId = request.DistrictId,
                            WardId = request.WardId,
                            Email = request.Email,
                            Phone = request.Phone,
                            Latitude = request.Latitude,
                            Longitude = request.Longitude,
                            UploadImage = HostAddress + GenAbsolutePath(relativePath),
                            Status = false,

                        };
                        result.Message = "Add Success";
                        _datacontext.EN_DeliveryDriver.Add(province);
                    }
                    else
                    {
                        var storeUpdate = _datacontext.EN_DeliveryDriver.Where(x => x.DeliveryDriverId == request.DeliveryDriverId).FirstOrDefault();
                        if (storeUpdate != null)
                        {
                            storeUpdate.CompleteName = request.CompleteName;
                            storeUpdate.Email = request.Email;
                            storeUpdate.ProvinceId = request.ProvinceId;
                            storeUpdate.DistrictId = request.DistrictId;
                            storeUpdate.WardId = request.WardId;
                            storeUpdate.Phone = request.Phone;
                            storeUpdate.Latitude = request.Latitude;
                            storeUpdate.Longitude = request.Longitude;
                            if (storeUpdate.UploadImage != request.UploadImage)
                            {
                                storeUpdate.UploadImage = HostAddress + GenAbsolutePath(relativePath);
                            }
                            storeUpdate.Status = request.Status;
                            result.Message = "Update Success";
                        }
                    }
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

        public async Task<BaseResponse<bool>> DeleteStore(StoreRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var province = await _datacontext.EN_Store.FirstOrDefaultAsync(x => x.ProvinceId == request.ProvinceId);
                _datacontext.EN_Store.Remove(province);
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
        public async Task<BaseResponse<StoreResponse>> SearchProvince(string provinceName)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var query = await _datacontext.EN_Store.SingleOrDefaultAsync(x => x.FullName == provinceName);
                result.Item = query.MapTo<StoreResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<StoreResponse>> TakeStoreByCuisineId(FilterStoreByCusineRequest filter)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                if(filter.CuisineId != 0)
                {
                    var query = _datacontext.EN_Store.AsQueryable();
                    query = query.Where(d => d.Cuisine.CuisineId == filter.CuisineId && d.Status == true);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    var resultList = FindNearestStores(data.MapTo<StoreResponse>(), filter.latitude, filter.longitude, 40).MapTo<StoreResponse>();
                    result.Data = (IList<StoreResponse>)resultList;
                }
                else
                {
                    var query = _datacontext.EN_Store.AsQueryable();
                    query = query.Where(d => d.Status == true);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    var resultList = FindNearestStores(data.MapTo<StoreResponse>(), filter.latitude, filter.longitude, 40).MapTo<StoreResponse>();
                    result.Data = (IList<StoreResponse>)resultList;
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(OrderHeaderFillterRequest request)
        {
            var result = new BaseResponse<OrderHeaderResponse> { };
            try
            {
                var query = _datacontext.EN_OrderHeader.AsQueryable();
                query = query.Where(d => d.UserId == request.Id ).OrderByDescending(d => d.CreationDate).ThenByDescending(d => d.Status == false);
                query = query.Where(d => d.OrderHeaderId.Contains(request.keyword)
                                 || d.FormatAddress.Contains(request.keyword)
                                 || d.OrderHeaderId.Contains(request.keyword)
                                 || d.RecipientName.Contains(request.keyword)
                                 || d.RecipientPhone.Contains(request.keyword));
                if (request.OrderStatus != 2)
                {
                    query = query.Where(x => x.Status.Equals(request.OrderStatus == 1 ? true : false));
                }
                if (request.startDate == DateTime.MinValue ||  request.endDate == DateTime.MaxValue)
                {
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    var resultList = data.MapTo<OrderHeaderResponse>();
                    result.Data = resultList;
                    result.Success = true;
                }
                else
                {
                    query = query.Where(x => x.CreationDate >= request.startDate && x.CreationDate <= request.endDate);
                    result.DataCount = await query.CountAsync();
                    var data = await query.ToListAsync();
                    var resultList = data.MapTo<OrderHeaderResponse>();
                    result.Data = resultList;
                    result.Success = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<StatisticalResponse>> TakeStatisticalByStoreId(StatisticalRequest request)
        {
            var result = new BaseResponse<StatisticalResponse> { };
            try
            {
                result.Item = new StatisticalResponse();
                result.Item.listChart = new List<StatisticalChart>();
                List<StatisticalChart> statisticalCharts = new List<StatisticalChart>();

                DateTime now = DateTime.Now; // get current date and time
                double? getStatisticalStoreDate = await _datacontext.EN_OrderHeader
                    .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Day == now.Day)
                    .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                result.Item.revenueDate = getStatisticalStoreDate ?? 0;

                //Doanh thu tuần
                int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

                var ordersInCurrentWeek = await _datacontext.EN_OrderHeader
                    .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Year == now.Year)
                    .ToListAsync();

                double? getStatisticalStoreWeek = ordersInCurrentWeek
                    .Where(x => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.CreationDate.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) == currentWeek)
                    .Sum(x => (double?)x.IntoMoney ?? 0);

                result.Item.revenueWeek = getStatisticalStoreWeek ?? 0;

                //Doanh thu tháng
                double? getStatisticalStoreMonth = await _datacontext.EN_OrderHeader
                .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == now.Month)
                .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                //Doanh thu năm
                double? getStatisticalStoreYear = await _datacontext.EN_OrderHeader
                .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Year == now.Year)
                .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                result.Item.revenueYear = getStatisticalStoreYear ?? 0;

                if (request.endDate == DateTime.MinValue || request.startDate == DateTime.MinValue)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        double? getStatisticalChartMonth = await _datacontext.EN_OrderHeader
                        .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == i)
                        .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                        result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                        StatisticalChart chart = new StatisticalChart()
                        {
                            revenueMonth = getStatisticalChartMonth,
                            nameMonth = $"Tháng " + i,
                        };
                        result.Item.listChart.Add(chart);
                    }
                }
                else
                {
                    var yearStart = request.startDate.Year;
                    var yearEnd = request.endDate.Year;
                    if(yearStart < yearEnd )
                    {
                        for (int i = request.startDate.Month; i <= 12; i++)
                        {
                            double? getStatisticalChartMonth = await _datacontext.EN_OrderHeader
                            .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == i && x.CreationDate.Value.Year == yearStart)
                            .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                            result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                            StatisticalChart chart = new StatisticalChart()
                            {
                                revenueMonth = getStatisticalChartMonth,
                                nameMonth = i + "/"+request.startDate.Year,
                            };
                            result.Item.listChart.Add(chart);
                        }
                        for (int i = 1; i <= request.endDate.Month; i++)
                        {
                            double? getStatisticalChartMonth = await _datacontext.EN_OrderHeader
                            .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == i && x.CreationDate.Value.Year == yearEnd)
                            .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                            result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                            StatisticalChart chart = new StatisticalChart()
                            {
                                revenueMonth = getStatisticalChartMonth,
                                nameMonth = i + "/" + request.endDate.Year,
                            };
                            result.Item.listChart.Add(chart);
                        }

                    }
                    else if (yearStart == yearEnd)
                    {
                        for (int i = request.startDate.Month; i <= request.endDate.Month; i++)
                        {
                            double? getStatisticalChartMonth = await _datacontext.EN_OrderHeader
                            .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == i && x.CreationDate.Value.Year == yearEnd)
                            .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                            result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                            StatisticalChart chart = new StatisticalChart()
                            {
                                revenueMonth = getStatisticalChartMonth,
                                nameMonth =  i+"/"+ yearStart,
                            };
                            result.Item.listChart.Add(chart);
                        }
                    }
                    else {

                    }
                }
                //Thong ke chart

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<OrderLineReponse>> GetListOrderLineDetails(string Id)
        {
            var result = new BaseResponse<OrderLineReponse> { };
            try
            {
                var query = _datacontext.EN_OrderLine.AsQueryable();
                query = query.Where(d => d.OrderHeaderId == Id);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = data.MapTo<OrderLineReponse>();
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ApproveStore(StoreRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                int newPass = rand.Next(100000, 999999);
                var ApproveStore = await _datacontext.EN_Store.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync();
                var checkAccount = await _datacontext.EN_Account.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync();
                if (checkAccount == null)
                {
                    var createACcount = new EN_Account()
                    {
                        UserId = request.UserId,
                        Username = request.Email,
                        Password = MD5Hash(Base64Encode(newPass.ToString())),
                        Status = true,
                        AccountId = 1,
                    };
                    _datacontext.EN_Account.Add(createACcount);

                    ApproveStore.Status = true;

                    try
                    {
                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.EnableSsl = true;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("chauvanrangdong4440@gmail.com", "wbswwxmptbmwfqpg");

                                MailMessage mail = new MailMessage();
                                mail.To.Add(request.Email);  // Email của khách hàng
                                mail.From = new MailAddress("chauvanrangdong4440@gmail.com");    mail.Subject = "BẠN ĐÃ ĐƯỢC CẤP MẬT KHẨU BÁN HÀNG";
                            mail.Body = $"Mật khẩu của bạn là: {newPass}.Vui lòng đăng nhập thay đổi mật khẩu mới. Trân trọng !";

                            smtp.Send(mail);
                        }
                    }
                    catch { }
                }
                else
                {
                    
                    ApproveStore.Status = !ApproveStore.Status;
                    if (ApproveStore.Status)
                    {
                        checkAccount.Password = MD5Hash(Base64Encode(newPass.ToString()));
                        try
                        {
                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.EnableSsl = true;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new NetworkCredential("chauvanrangdong4440@gmail.com", "wbswwxmptbmwfqpg");

                                MailMessage mail = new MailMessage();
                                mail.To.Add(request.Email);  // Email của khách hàng
                                mail.From = new MailAddress("chauvanrangdong4440@gmail.com"); mail.Subject = "BẠN ĐÃ ĐƯỢC CẤP MẬT KHẨU BÁN HÀNG";
                                mail.Body = $"Mật khẩu của bạn là: {newPass}.Vui lòng đăng nhập thay đổi mật khẩu mới. Trân trọng !";

                                smtp.Send(mail);
                            }
                        }
                        catch { }
                    }
                 
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
        public async Task<BaseResponse<bool>> ApproveDelvery(DeliveryDriverRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var ApproveStore = await _datacontext.EN_DeliveryDriver.Where(x => x.DeliveryDriverId == request.DeliveryDriverId).FirstOrDefaultAsync();
                if (ApproveStore != null)
                {
                    if (!ApproveStore.Status)
                    {
                        int newPass = rand.Next(100000, 999999);
                        ApproveStore.Username = request.Email;
                        ApproveStore.Password = MD5Hash(Base64Encode(newPass.ToString()));
                        ApproveStore.Status = true;
                        try
                        {
                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.EnableSsl = true;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new NetworkCredential("chauvanrangdong4440@gmail.com", "wbswwxmptbmwfqpg");
                                MailMessage mail = new MailMessage();
                                mail.To.Add(request.Email);  // Email của khách hàng
                                mail.From = new MailAddress("chauvanrangdong4440@gmail.com");
                                mail.Subject = "ETATINGNOW | BẠN ĐÃ ĐƯỢC CẤP MẬT KHẨU TÀI KHOẢN";
                                mail.Body = $"Mật khẩu của bạn là: {newPass}.Vui lòng đăng nhập vào ứng dụng và thay đổi mật khẩu mới. Trân trọng!";

                                smtp.Send(mail);
                            }
                            result.Message = "Success!";
                            result.Success = true;
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        result.Success = true;
                        ApproveStore.Status = !ApproveStore.Status;
                    }
                }
                else
                {
                    result.Message = "Id Not Found!";
                }
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        private string LocalMapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        public string GenAbsolutePath(string relativePath)
        {
            var systemSettings = _settingService.LoadSetting<SystemSettings>();
            var path = systemSettings.Domain + relativePath.Replace("\\", "/");
            path = path.Replace("//", "/");
            return path;
        }

        public bool CheckImageFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (ext == null)
                return false;
            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }

        //Tính khoảng cách giữa 2 toạ độ
        public static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // đường kính trái đất (km)
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var distance = R * c; // khoảng cách giữa hai điểm (km)
            return Math.Round(distance, 1);
        }

        public static double ToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }


        public static List<StoreResponse> FindNearestStores(List<StoreResponse> stores, double lat, double lng, int count)
        {
            var nearestStores = new List<StoreResponse>();

            // Tính khoảng cách từ điểm đầu tiên đến tất cả các điểm còn lại trong danh sách
            foreach (var store in stores)
            {
                double Lat = Math.Round(store.Latitude, 7);
                double Lon = Math.Round(store.Longitude, 7);
                var distance = Distance(/*lat*/lat, /*lng*/ lng, Lat, Lon);
                store.Distance = distance;
                //store.Time = CalculateTime(distance); // Assuming you have a function to calculate time based on distance
            }

            foreach(var fillStore in stores)
            {
                if(fillStore.Distance <=10 )
                    nearestStores.Add(fillStore);
            }
            nearestStores = nearestStores.OrderBy(store => store.Distance).ToList();

            return nearestStores;
        }

        private static double CalculateTime(double distance)
        {
            // Assuming an average speed of 50 km/h (31 mph)
            double averageSpeed = 40; // km/h
            double time = distance / averageSpeed;
            double timeInMinutes = Math.Round(time * 60, 0);
            return timeInMinutes;
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Xác nhận đơn hàng
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<bool>> ApproveOrder(OrderHeaderRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var orderHeader = await _datacontext.EN_OrderHeader
                    .Where(x => x.OrderHeaderId == request.OrderHeaderId)
                    .FirstOrDefaultAsync();
                var store = await _datacontext.EN_Store
                    .Where(x => x.UserId == request.UserId)
                    .FirstOrDefaultAsync();
                var account = await _datacontext.EN_Customer
                  .Where(x => x.CustomerId == request.CustomerId)
                  .FirstOrDefaultAsync();
                if (orderHeader != null  && store != null)
                {
                    if (!orderHeader.Status)
                    {
                        orderHeader.Status = true;
                        // Gửi thông báo đến email của khách hàng
                        if(account.Email != null)
                        {
                            try
                            {
                                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    smtp.EnableSsl = true;
                                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Credentials = new NetworkCredential("chauvanrangdong4440@gmail.com", "wbswwxmptbmwfqpg");

                                    MailMessage mail = new MailMessage();
                                    mail.To.Add(account.Email);  // Email của khách hàng
                                    mail.From = new MailAddress("chauvanrangdong4440@gmail.com");  // Email của bạn
                                    mail.Subject = "EATINGNOW | ĐƠN HÀNG ĐƯỢC DUYỆT";
                                    mail.Body = "Subject: Xác nhận Đơn Hàng: Đã Duyệt Thành Công 🎉\r\n\r\nChào quý khách hàng thân mến,\r\n\r\nChúc mừng! Đơn hàng của bạn đã được xác nhận và duyệt thành công tại EatingNow. Dưới đây là chi tiết đơn hàng để quý khách tham khảo:\r\n\r\n━━━━━━━━━━━━━━━\r\n📦 THÔNG TIN ĐƠN HÀNG:\r\n━━━━━━━━━━━━━━━\r\n📅 Ngày tạo đơn hàng: " + orderHeader.CreationDate + "\r\n💰 Tổng giá trị sản phẩm: " + orderHeader.TotalAmt + "\r\n🚚 Phí giao hàng: " + orderHeader.TransportFee + "\r\n💳 Tổng cộng cần thanh toán: " + orderHeader.IntoMoney + "\r\n\r\n🔗 Quý khách có thể xem chi tiết đơn hàng tại: http://localhost:3001/order/"+orderHeader.OrderHeaderId+"\r\n\r\n📞 Liên hệ với chúng tôi qua số điện thoại của Chủ cửa hàng " + store.OwnerName + " tại " + store.Phone + " nếu cần hỗ trợ hoặc có câu hỏi liên quan đến đơn hàng.\r\n\r\n🛍️ Chúng tôi rất biết ơn vì quý khách đã lựa chọn EatingNow để mua sắm. Chúc quý khách có một trải nghiệm mua sắm thú vị!\r\n\r\nTrân trọng,\r\nEatingNow Team 🍔\U0001f6d2\r\n━━━━━━━━━━━━━━━\r\n";
                                    smtp.Send(mail);
                                }
                            }
                            catch
                            {
                                result.Success = false;
                            }
                        }
                        var newnotification = new EN_CustomerNotifications
                        {
                            CustomerID = request.CustomerId,
                            NotificationDate = DateTime.Now,
                            SenderName = "HỆ THỐNG",
                            Message = "Đơn hàng " + request.OrderHeaderId + " đã được được xác nhận",
                            IsRead = false,
                            Action_Link = "/order/"+request.OrderHeaderId
                        };
                        _datacontext.EN_CustomerNotifications.Add(newnotification);
                        result.Success = true;
                    }   
                    else
                    {
                        var newnotification = new EN_CustomerNotifications
                        {
                            CustomerID = request.CustomerId,
                            NotificationDate = DateTime.Now,
                            SenderName = "HỆ THỐNG",
                            Message = "Đơn hàng " + request.OrderHeaderId + " đã được hủy",
                            IsRead = false,
                            Action_Link = "/order/" + request.OrderHeaderId
                        };
                        _datacontext.EN_CustomerNotifications.Add(newnotification);
                        orderHeader.Status = false;
                        result.Success = true;
                    }
                    await _datacontext.SaveChangesAsync();

                }
                else
                {
                    result.Message = "Không tìm thấy đơn hàng hoặc tài khoản.";
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }

            return result;
        }


        public async Task<BaseResponse<OrderHeaderResponse>> TakeAllOrder(OrderHeaderRequest request)
        {
            var result = new BaseResponse<OrderHeaderResponse>();
            try
            {
                var query = _datacontext.EN_OrderHeader.AsQueryable();
                if (!string.IsNullOrEmpty(request.Term))
                {
                    query = query.Where(x => x.OrderHeaderId.Contains(request.Term)
                        || x.EN_Customer.CompleteName.Contains(request.Term)
                        || x.EN_Customer.CompleteName.Contains(request.Term)
                        || x.EN_Customer.Phone.Contains(request.Term)
                        || x.EN_Customer.Email.Contains(request.Term)
                        || x.FormatAddress.Contains(request.Term)
                        || x.NameAddress.Contains(request.Term)
                    );
                }
                result.DataCount = await query.CountAsync();
                if (request.PageSize != 0)
                {
                    query = query.OrderByDescending(d => d.CreationDate).ThenByDescending(d => !d.Status)
                        .Skip(request.Page * request.PageSize)
                        .Take(request.PageSize);
                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<OrderHeaderResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
                result.Success = false; // Set Success to false in case of an exception
            }

            return result;
        }

        /// <summary>
        /// Lấy tất cả các sản phẩm của khách hàng đã mua
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<BaseResponse<OrderLineReponse>> TakeAllOrderLineByCustomerId(EN_CustomerRequest request)
        {
            var result = new BaseResponse<OrderLineReponse>();
            try
            {
                var query = _datacontext.EN_OrderLine
                    .Where(odl => odl.EN_OrderHeader.CustomerId == request.CustomerId);

                if (!string.IsNullOrEmpty(request.Term))
                {
                    var term = request.Term.ToLower();
                    query = query.Where(x =>
                        x.OrderHeaderId.ToLower().Contains(term) ||
                        x.Description.ToLower().Contains(term) ||
                        x.FoodName.ToLower().Contains(term));
                }

                result.DataCount = await query.CountAsync();

                if (request.PageSize > 0)
                {
                    query = query.OrderBy(d => d.OrderHeaderId)
                        .Skip(request.Page * request.PageSize)
                        .Take(request.PageSize);
                }

                var data = await query.ToListAsync();
                result.Data = data.MapTo<OrderLineReponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<StoreResponse>> TakeStoreLocation(StoreRequest request)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var query = _datacontext.EN_Store.Where(x => x.UserId.Equals(request.UserId)).AsQueryable();
                result.DataCount = query.ToList().Count();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<StoreResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> RemoveDriverr(DeliveryDriverRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var driver = await _datacontext.EN_DeliveryDriver.SingleOrDefaultAsync(x => x.DeliveryDriverId == request.DeliveryDriverId);
                try
                {
                    if (driver != null)
                    {
                        _datacontext.EN_DeliveryDriver.Remove(driver);
                        await _datacontext.SaveChangesAsync();
                        result.Success = true;
                        result.Message = "Success!";
                    }
                }
                catch
                {
                    result.Message = "Không thể xóa!";
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {

                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<ListOfProductSold>> TakeLitsFoodSold(int UserId)
        {
            var result = new BaseResponse<ListOfProductSold>();
            try
            {
                var store =await  _datacontext.EN_Store.Where(x => x.UserId.Equals(UserId)).FirstOrDefaultAsync();
                if(store != null)
                {
                    var data = new List<ListOfProductSold>();
                    var query = from od in _datacontext.EN_OrderHeader
                                join odl in _datacontext.EN_OrderLine on od.OrderHeaderId equals odl.OrderHeaderId
                                join ctm in _datacontext.EN_Customer on od.CustomerId equals ctm.CustomerId
                                join fl in _datacontext.EN_FoodList on odl.FoodListId equals fl.FoodListId
                                where od.UserId == UserId
                                group fl by new { fl.FoodName, fl.FoodListId } into g
                                select new ListOfProductSold
                                {
                                    FoodListId = g.Key.FoodListId,
                                    FoodName = g.Key.FoodName,
                                    FoodCount = g.Count()
                                };

                    var resultquery = await query.Take(10).ToListAsync(); 
                    result.Message = "Get Success";
                    result.Success = true;
                    result.Data = resultquery;
                    result.DataCount = resultquery.Count;
                }
                else
                {
                    result.Message = "Cửa hàng không có trên hệ thống";
                    result.Success = false;
                }

            }catch(Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
    }
}
