using AI.FoodList;
using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Constants;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.FoodList;
using DaiPhucVinh.Shared.Store;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Falcon.Web.Core.Log;
using Falcon.Web.Core.Settings;
using Microsoft.AspNet.SignalR;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
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
        Task<BaseResponse<FoodListResponse>> TakeFavoriteFoodListOfUser(EN_CustomerLocationRequest request);
        Task<BaseResponse<FoodListSearchResponse>> SearchFoodListByUser(string keyword, float latitude, float longitude );
    }
    public class FoodListService : IFoodListService
    {

        const string SubPath = "\\uploads\\DaiPhucVinh\\Image";
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly RecommendedFoodList _recommendPrediction;

        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public FoodListService(DataContext datacontext, ILogService logService, ICommonService commonService, ILogger logger, ISettingService settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
            _settingService = settingService;
            _logger = logger;
            _recommendPrediction = new RecommendedFoodList();
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
                if(data.Count > 0)
                {
                    foreach (var item in query)
                    {
                        if (item.Items != null && item.Items.Count > 0)
                            data.AddRange(item.Items.MapTo<FoodListResponse>());
                    }
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
                            QuantitySupplied = request.QuantitySupplied,
                            UploadImage = HostAddress + GenAbsolutePath(relativePath),
                            Description = request.Description,
                            UserId = request.UserId,
                            Status = request.Status,
                            Hint = request.Hint,
                            IsNew = request.IsNew,
                            IsNoiBat = request.IsNoiBat,
                            ExpiryDate = request.ExpiryDate != DateTime.MinValue ? request.ExpiryDate : null,
                            Qtycontrolled = request.Qtycontrolled,
                            QtySuppliedcontrolled = request.QtySuppliedcontrolled
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
                            foodList.Description = request.Description;
                            foodList.UserId = request.UserId;
                            foodList.Status = request.Status;
                            foodList.Hint = request.Hint;
                            foodList.IsNew = request.IsNew;
                            foodList.IsNoiBat = request.IsNoiBat;
                            foodList.ExpiryDate = request.ExpiryDate;
                            foodList.QuantitySupplied = request.QuantitySupplied;
                            foodList.Qtycontrolled = request.Qtycontrolled;
                            foodList.QtySuppliedcontrolled = request.QtySuppliedcontrolled;

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
                    foodList.Hint = request.Hint;
                    foodList.IsNew = request.IsNew;
                    foodList.IsNoiBat = request.IsNoiBat;
                    foodList.ExpiryDate = request.ExpiryDate;
                    foodList.QuantitySupplied = request.QuantitySupplied;
                    foodList.Qtycontrolled = request.Qtycontrolled;
                    foodList.QtySuppliedcontrolled = request.QtySuppliedcontrolled;
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
                foodList.IsNoiBat = !foodList.IsNoiBat;
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
                foodList.IsNew = !foodList.IsNew;
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
        /// API gợi ý món ăn cho khách hàng dựa vào lịch sử mua hàng và đánh giá của người dùng sử dụng Machine Learning (ML.NET)
        /// Thuật toán Matrix Factorization
        /// của khách hàng và lọc ra những cửa hàng gần với tọa độ vị trí của khách hàng nhất trong phạm vi 10 km
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<FoodListResponse>> TakeRecommendedFoodList(EN_CustomerLocationRequest request)
        {
            var result = new BaseResponse<FoodListResponse>();
            try
            {
                var newfoodlist = new List<FoodList_Store>();

                var allStores = _datacontext.EN_Store
                .Where(x => x.Status == true)
                .ToList();

                var foodlistNearS = (from store in allStores
                                     where Distance(request.Latitude, request.Longittude, store.Latitude, store.Longitude) <= 10
                                     from food in _datacontext.EN_FoodList
                                     where food.UserId == store.UserId
                                     select new FoodList_Store
                                     {
                                         foodItem = food,
                                         Storeitem = store
                                     }).ToList();


                var inputDatas = new List<InputData>();

                if (request.CustomerId != null)
                {
                    foreach (var food in foodlistNearS)
                    {
                        inputDatas.Add(new InputData
                        {
                            CustomerId = request.CustomerId,
                            FoodListId = food.foodItem.FoodListId,
                        });
                    }

                    List<ResultModel> data = _recommendPrediction.Predicvalue(inputDatas);

                    if (data.Count > 5)
                    {
                        foreach (var inputData in data)
                        {
                            var matchingFood = foodlistNearS.FirstOrDefault(x => x.foodItem.FoodListId == inputData.FoodListId);
                            if (matchingFood != null)
                            {
                                newfoodlist.Add(matchingFood);
                            }
                        }
                        result.Data = newfoodlist.MapTo<FoodListResponse>();
                        result.DataCount = newfoodlist.Count;
                    }
                    else
                    {
                        result.Data = foodlistNearS.MapTo<FoodListResponse>();
                        result.DataCount = foodlistNearS.Count;
                    }
                }
                else
                {
                    // Show all available products
                    result.Data = foodlistNearS.MapTo<FoodListResponse>();
                    result.DataCount = foodlistNearS.Count;
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

        public async Task<BaseResponse<FoodListResponse>> TakeFavoriteFoodListOfUser(EN_CustomerLocationRequest request)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var data = new List<EN_FoodList>();
                var favoriteFoods = _datacontext.EN_FavoriteFoods.Where(s => s.CustomerID.Equals(request.CustomerId)).ToList();
                if (favoriteFoods.Count > 0)
                {
                    foreach (var foodFvr in favoriteFoods)
                    {
                        var food = await _datacontext.EN_FoodList.Where(x => x.FoodListId.Equals(foodFvr.FoodID)).OrderBy(d => d.FoodListId).Take(10).ToListAsync();
                        data.AddRange(food);
                    }
                    result.Data = data.MapTo<FoodListResponse>();
                }
                else
                    result.Data = null;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }


        public async Task<BaseResponse<FoodListSearchResponse>> SearchFoodListByUser(string keyword, float latitude, float longitude)
        {
            var result = new BaseResponse<FoodListSearchResponse> { };
            try
            {
                /*
                var listStore = await _datacontext.EN_Store.Include(x => x.Cuisine).Where(
                    x =>
                    x.FullName.Contains(keyword)
                    || x.OwnerName.Contains(keyword)    
                    || x.Description.Contains(keyword)
                    || x.Phone.Contains(keyword)
                    || x.Email.Contains(keyword)
                    || x.Cuisine.Name.Contains(keyword)
                    ).ToListAsync();*/
                var listStore = await _datacontext.EN_Store.Include(x => x.Cuisine).ToListAsync();
                var listResult = new List<FoodListSearchResponse>();
                var resulStoretList = FindNearestStores(listStore.MapTo<StoreResponse>(), latitude, longitude, 20).MapTo<StoreResponse>();
                // var storeresults = BinarySearch(listStore, keyword);
                foreach (var store in resulStoretList)
                {
                    var resultFoodListSearch = new FoodListSearchResponse();
                   
                    var foodlist = await _datacontext.EN_FoodList.Include(x => x.Category)
                        .Where( x => x.UserId.Equals(store.UserId) && x.FoodName.Contains(keyword)).ToListAsync();
                    if(foodlist.Count > 0)
                    {
                        resultFoodListSearch.storeinFo = store;
                        resultFoodListSearch.foodList = foodlist.Take(4).MapTo<FoodListResponse>();
                        listResult.Add(resultFoodListSearch);
                    }
                    else
                    {
                        if (store.FullName.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            || store.OwnerName.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            || store.Description.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            || store.Phone.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0    
                            || store.Email.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            || store.Cuisine.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            || store.Address.ToLower().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                            )
                        {
                            resultFoodListSearch.storeinFo = store;
                            resultFoodListSearch.foodList = new List<FoodListResponse>();
                            listResult.Add(resultFoodListSearch);
                        }
                    }
                }
                /*
                foreach (var store in resultList)
                {
                    var resultFoodListSearch = new FoodListSearchResponse();
                    resultFoodListSearch.storeinFo = store.MapTo<StoreResponse>();
                    var foodlistNearS = (from stores in resultList
                                         from food in _datacontext.EN_FoodList
                                         where food.UserId == store.UserId
                                         select new FoodListResponse
                                         {
                                             FoodListId = food.FoodListId,
                                             FoodName = food.FoodName,
                                             storeName = store.FullName,
                                             Price = food.Price,
                                             qty = food.qty,
                                             UploadImage = food.UploadImage,
                                             Description = food.Description,
                                             UserId = food.UserId,
                                             Status = food.Status,
                                             Hint = food.Hint,
                                             IsNew = food.IsNew,
                                             IsNoiBat = food.IsNoiBat,
                                             QuantitySupplied = food.QuantitySupplied,
                                             ExpiryDate = food.ExpiryDate,
                                             Qtycontrolled = food.Qtycontrolled,
                                             QtySuppliedcontrolled = food.QtySuppliedcontrolled,
                                             Latitude = store.Latitude,
                                             Longitude = store.Longitude,
                                         }).ToList();

                    resultFoodListSearch.foodList = foodlist.Take(4).MapTo<FoodListResponse>();
                    listResult.Add(resultFoodListSearch);
                }
                
              */

                result.Success = true;
                result.Data = listResult;
                result.Message = "SearchSuccess";
                result.DataCount = listStore.Count;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public static double CalculateSimilarity(string str1, string str2)
        {
            // Chuyển đổi cả hai chuỗi sang chữ thường
            str1 = str1.ToLower();
            str2 = str2.ToLower();

            int minLength = Math.Min(str1.Length, str2.Length);
            int commonChars = 0;

            // Đếm số lượng ký tự chung
            for (int i = 0; i < minLength; i++)
            {
                if (str1[i] == str2[i])
                {
                    commonChars++;
                }
            }

            // Tính tỷ lệ giống nhau
            double similarity = (double)commonChars / Math.Max(str1.Length, str2.Length);

            return similarity;
        }
        // Thuật toán tìm kiếm nhị phân
        public static List<T> BinarySearch<T>(List<T> list, string query) where T : class
        {
            List<T> listResult = new List<T>();

            // Kiểm tra trường hợp đặc biệt
            if (list == null || list.Count == 0 || string.IsNullOrEmpty(query))
            {
                return listResult;
            }

            // Sắp xếp danh sách
            list.Sort((a, b) => {
                if (a is EN_FoodList && b is EN_FoodList)
                {
                    return string.Compare((a as EN_FoodList).FoodName, (b as EN_FoodList).FoodName);
                }
                else if (a is EN_Store && b is EN_Store)
                {
                    return string.Compare((a as EN_Store).FullName, (b as EN_Store).FullName);
                }
                else
                {
                    // Thực hiện sắp xếp theo các tiêu chí khác nếu cần
                    return 0;
                }
            });

            // Chuyển đổi truy vấn sang chữ thường
            query = query.ToLower();

            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (list[mid] is EN_FoodList)
                {
                    EN_FoodList product = list[mid] as EN_FoodList;
                    string foodName = product.FoodName.ToLower();
                    string description = product.Description.ToLower();
                    string category = product.Category.CategoryName.ToLower();

                    if (foodName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
    description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
    category.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        listResult.Add(product as T);
                    }


                    // So sánh tên sản phẩm với truy vấn
                    int comparison = string.Compare(foodName, query);

                    // Cập nhật chỉ mục left và right dựa trên kết quả so sánh
                    if (comparison < 0)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }
                else if (list[mid] is EN_Store)
                {
                    EN_Store store = list[mid] as EN_Store;
                    string fullName = store.FullName.ToLower();
                    string description = store.Description.ToLower();
                    string cuisine = store.Cuisine.Name.ToLower();

                    if (fullName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        cuisine.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        listResult.Add(store as T);
                    }

                    // So sánh tên cửa hàng với truy vấn
                    int comparison = string.Compare(fullName, query);
                    int comparisondescription = string.Compare(description, query);
                    int comparisoncuisine = string.Compare(cuisine, query);
                    /*
                    // Cập nhật chỉ mục left và right dựa trên kết quả so sánh
                    if (comparison < 0 || comparisondescription < 0 || comparisoncuisine < 0)
                    {
                        left = mid + 1;
                    }
                    else if (comparison > 0 || comparisondescription > 0 || comparisoncuisine > 0)
                    {
                        right = mid - 1;
                    }
                    else
                    {
                        // Khi các kết quả so sánh bằng nhau, ta cũng cần di chuyển chỉ mục left và right để duyệt tiếp
                        left = mid + 1;
                        right = mid - 1;
                    }
                    */
                    // Cập nhật chỉ mục left và right dựa trên kết quả so sánh
                    if (comparison < 0)
                    {
                        left = mid + 1;
                    }
                    else
                    {
                        right = mid - 1;
                    }
                }
            }

            return listResult;
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

            foreach (var fillStore in stores)
            {
                if (fillStore.Distance <= 10)
                    nearestStores.Add(fillStore);
            }
            nearestStores = nearestStores.OrderBy(store => store.Distance).ToList();

            return nearestStores;
        }

    }
}
