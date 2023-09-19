using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.FoodList;
using Falcon.Web.Core.Log;
using Falcon.Web.Core.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace DaiPhucVinh.Services.MainServices.FoodList
{
    public interface IFoodListService
    {
        string HostAddress { get; }
        Task<BaseResponse<bool>> CreateFoodItem(FoodListRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdateFoodListHaveImage(FoodListRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdateFoodListNotImage(FoodListRequest request); 
        Task<BaseResponse<bool>> DeleteFoodList(FoodListRequest request);
        Task<BaseResponse<FoodListResponse>> TakeFoodListById(int FoodListId);
        Task<BaseResponse<bool>> ChangeIsNoiBatFoodList(FoodListRequest request);
        Task<BaseResponse<bool>> ChangeIsNewFoodList(FoodListRequest request);
        Task<BaseResponse<FoodListResponse>> TakeFoodListByHint();
        Task<BaseResponse<FoodListResponse>> TakeBestSeller();
        Task<BaseResponse<FoodListResponse>> TakeNewFood();
        Task<BaseResponse<FoodListResponse>> TakeRecommendedFoodList(EN_CustomerLocationRequest request);

    }
    public class FoodListService : IFoodListService
    {

        const string SubPath = "\\uploads\\DaiPhucVinh\\Image";
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public FoodListService(DataContext datacontext, ILogService logService, ICommonService commonService, ILogger logger, ISettingService settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
            _settingService = settingService;
            _logger = logger;
        }

        public async Task<BaseResponse<FoodListResponse>> TakeFoodListByHint()
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var data = await _datacontext.EN_FoodList.Where(x => x.Hint != null).OrderBy(d => d.Hint).Take(10).ToListAsync();
                result.Data = data.MapTo<FoodListResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<FoodListResponse>> TakeBestSeller()
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var query = _datacontext.EN_FoodList.Where(s => s.IsNoiBat == true).GroupBy(x => x.UserId).Select(d => new
                {
                    UserId = d.Key,
                    Items = d.Take(2).ToList()
                }).ToList();
                var data = new List<FoodListResponse>();
                foreach (var item in query)
                {
                    if (item.Items != null && item.Items.Count > 0)
                        data.AddRange(item.Items.MapTo<FoodListResponse>());
                }
                result.Data = data;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<FoodListResponse>> TakeNewFood()
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var query = _datacontext.EN_FoodList.Where(s => s.IsNew == true).GroupBy(x => x.UserId).Select(d => new
                {
                    UserId = d.Key,
                    Items = d.Take(2).ToList()
                }).ToList();
                var data = new List<FoodListResponse>();
                foreach (var item in query)
                {
                    if (item.Items != null && item.Items.Count > 0)
                        data.AddRange(item.Items.MapTo<FoodListResponse>());
                }
                result.Data = data;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateFoodItem(FoodListRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                using (MemoryStream ms = new MemoryStream())
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

                    var relativePath = Path.Combine(storageFolder, newFileName);

                    if (request.FoodListId == 0)
                    {
                        if (request.Description == "")
                        {
                            request.Description = null;
                        }
                        var foodList = new EN_FoodList
                        {
                            FoodListId = request.FoodListId,
                            CategoryId = request.CategoryId,
                            FoodName = request.FoodName,
                            Price = request.Price,
                            qty = request.qty,
                            UploadImage = HostAddress + GenAbsolutePath(relativePath),
                            Description = request.Description,
                            UserId = request.UserId,
                            Status = request.Status,
                        };
                        _datacontext.EN_FoodList.Add(foodList);
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
        public async Task<BaseResponse<bool>> UpdateFoodListHaveImage(FoodListRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                using (MemoryStream ms = new MemoryStream())
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

                    var relativePath = Path.Combine(storageFolder, newFileName);

                    if (request.FoodListId > 0)
                    {
                        if (request.Description == "")
                        {
                            request.Description = null;
                        }
                        var foodList = await _datacontext.EN_FoodList.FindAsync(request.FoodListId);
                        {
                            foodList.FoodListId = request.FoodListId;
                            foodList.CategoryId = request.CategoryId;
                            foodList.FoodName = request.FoodName;
                            foodList.Price = request.Price;
                            foodList.qty = request.qty;
                            foodList.UploadImage = HostAddress + GenAbsolutePath(relativePath);
                            foodList.Description = request.Description;
                            foodList.UserId = request.UserId;
                            foodList.Status = request.Status;
                        };
                    }
                    await _datacontext.SaveChangesAsync();
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
        public async Task<BaseResponse<bool>> UpdateFoodListNotImage(FoodListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Description == "")
                {
                    request.Description = null;
                }
                var foodList = await _datacontext.EN_FoodList.FindAsync(request.FoodListId);
                {
                    foodList.FoodListId = request.FoodListId;
                    foodList.CategoryId = request.CategoryId;
                    foodList.FoodName = request.FoodName;
                    foodList.Price = request.Price;
                    foodList.qty = request.qty;
                    foodList.Description = request.Description;
                    foodList.UserId = request.UserId;
                    foodList.Status = request.Status;
                };
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
        public async Task<BaseResponse<bool>> DeleteFoodList(FoodListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var foodList = await _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId == request.FoodListId);
                _datacontext.EN_FoodList.Remove(foodList);
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
        public async Task<BaseResponse<bool>> ChangeIsNoiBatFoodList(FoodListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var foodList = await _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId == request.FoodListId);
                foodList.IsNoiBat = !request.IsNoiBat;
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
        public async Task<BaseResponse<bool>> ChangeIsNewFoodList(FoodListRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var foodList = await _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId == request.FoodListId);
                foodList.IsNew = !request.IsNew;
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
        public async Task<BaseResponse<FoodListResponse>> TakeFoodListById(int FoodListId)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var data = await _datacontext.EN_FoodList.FindAsync(FoodListId);
                result.Item = data.MapTo<FoodListResponse>();
                result.Success = true;
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
        /// <summary>
        /// Rạng Đông 
        /// API gọi ý món ăn yêu thích dựa trên lịch sử mua hàng của khách hàng. Lọc ra những loại món ăn yêu thích
        /// của khách hàng và lọc ra những cửa hàng gần với tọa độ vị trí của khách hàng nhất trong phạm vi 8 km
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<FoodListResponse>> TakeRecommendedFoodList(EN_CustomerLocationRequest request)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var recommendedFoods = new List<EN_FoodList>();
                // Lấy danh sách các cửa hàng 
                var stores = _datacontext.EN_Store.Where(x => x.Status == true).ToList();
                // Duyệt các cửa hàng có vị trí gàn với tọa độ người dùng và khoảng cách <= 8 Km
                var newStores = new List<EN_Store>();
                foreach (   var store in stores)
                {
                    var distance = Distance(request.Latitude, request.Longittude, store.Latitude, store.Longitude);
                    if (distance <= 10)
                    {
                        newStores.Add(store);
                    }
                }

                // Nếu khách hàng đã đăng nhập
                if (request.CustomerId != null)
                {
                    // Lịch sử mua của khách hàng
                    var query = from orderLine in _datacontext.EN_OrderLine
                                join orderHeader in _datacontext.EN_OrderHeader on orderLine.OrderHeaderId equals orderHeader.OrderHeaderId
                                where orderHeader.CustomerId == request.CustomerId
                                select orderLine;
                    var orderlines = query.ToList();

                    if(orderlines.Count > 0)
                    {
                        // lấy Danh sách tên các loại món ăn yêu thich dựa vào phân tích 
                        var userPreferences_nameCatagoryfood = AnalyzeUserPreferences(orderlines);
                        // Lấy danh sách món ăn đề xuất dựa trên loại món ăn yêu thích, gần với  tọa độ người dùng và đánh giá của cửa hàng >= 4*
                        foreach(var catagoryName in userPreferences_nameCatagoryfood)
                        {
                            foreach(var store in newStores)
                            {
                                // sử dụng so sánh like % trong SQL
                                var results = _datacontext.EN_FoodList
                               .Where(x => x.Category.CategoryName.Contains(catagoryName.CategoryName) && x.UserId == store.UserId)
                               .OrderBy(x => x.Price).ToList();
                                recommendedFoods.AddRange(results);
                            }    
                        }
                        //
                        var fianlrecommendFoods = recommendedFoods
                          .GroupBy(x => x.FoodListId)
                          .Select(group => group.First()) // Lấy một lần xuất hiện đầu tiên của mỗi nhóm
                          .ToList();
                        result.Data = fianlrecommendFoods.MapTo<FoodListResponse>();
                        result.DataCount = fianlrecommendFoods.Count;
                    }
                    else
                    {
                        foreach (var store in newStores)
                        {
                            var results = _datacontext.EN_FoodList.Where(x => x.UserId == store.UserId).OrderBy(x => x.Price).ToList();
                            recommendedFoods.AddRange(results);
                        }
                        result.Data = recommendedFoods.MapTo<FoodListResponse>();
                        result.DataCount = recommendedFoods.Count;

                    }
                    result.Success = true;
                }   
                // Nếu khách hàng là người mới chưa có tài khoản
                else
                {
                    foreach (var store in newStores)
                    {
                        var results = _datacontext.EN_FoodList.Where(x => x.UserId == store.UserId).OrderBy(x => x.Price).ToList();
                        recommendedFoods.AddRange(results);
                    }
                    result.Data = recommendedFoods.MapTo<FoodListResponse>();
                    result.DataCount = recommendedFoods.Count;
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



        // Phân tích dữ liệu sở thích món ăn của người dùng 
        public List<EN_CategoryList> AnalyzeUserPreferences(List<EN_OrderLine> orderLines)
        {
            var userPreferences = orderLines
                .GroupBy(ol => ol.CategoryId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .ToList();

            var Catagorys = new List<EN_CategoryList>();
            foreach (var category in userPreferences)
            {
                var results = _datacontext.EN_CategoryList.Where(x => x.CategoryId == category).ToList();
                Catagorys.AddRange(results);
            }
            return Catagorys;
        }



        public static double ToRadians(double degree)
        {
            return degree * Math.PI / 180;
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

    }
}
