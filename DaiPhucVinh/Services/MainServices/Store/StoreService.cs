using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.Common;
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

namespace DaiPhucVinh.Services.MainServices.Province
{
    public interface IStoreService
    {
        Task<BaseResponse<StoreResponse>> TakeAllStore(StoreRequest request);
        Task<BaseResponse<bool>> CreateNewStore(StoreRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdateNewStore(StoreRequest request);
        Task<BaseResponse<bool>> DeleteStore(StoreRequest request);
        Task<BaseResponse<StoreResponse>> TakeStoreById(int Id);
        Task<BaseResponse<StoreResponse>> TakeStoreByCuisineId(FilterStoreByCusineRequest filter);
        Task<BaseResponse<CategoryListResponse>> TakeCategoryByStoreId(int Id);
        Task<BaseResponse<FoodListResponse>> TakeFoodListByStoreId(int Id);
        Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(int Id);
        Task<BaseResponse<bool>> ApproveStore(StoreRequest request);
        Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int Id);
        Task<BaseResponse<OrderLineReponse>> GetListOrderLineDetails(string Id);
        Task<BaseResponse<StatisticalResponse>> TakeStatisticalByStoreId(StatisticalRequest request);
        Task<BaseResponse<StoreResponse>> TakeStoreByUserLogin(FilterStoreByCusineRequest filter);
        Task<BaseResponse<StoreResponse>> TakeStoreByCuisineUserLogin(FilterStoreByCusineRequest filter);
        Task<BaseResponse<FoodListResponse>> PostAllFoodListByStoreId(SimpleUserRequest request);
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
        public async Task<BaseResponse<StoreResponse>> TakeAllStore(StoreRequest request)
        {
            var result = new BaseResponse<StoreResponse> { };
            try
            {
                var query = _datacontext.EN_Store.AsQueryable();
                query = query.OrderBy(d => d.FullName);
                result.DataCount = await query.CountAsync();
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
        public async Task<BaseResponse<CategoryListResponse>> TakeCategoryByStoreId(int Id)
        {
            var result = new BaseResponse<CategoryListResponse> { };
            try
            {
                var query = _datacontext.EN_CategoryList.AsQueryable();
                query = query.Where(d => d.Store.UserId == Id);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = data.MapTo<CategoryListResponse>();
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
        public async Task<BaseResponse<FoodListResponse>> TakeFoodListByStoreId(int Id)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var query = _datacontext.EN_FoodList.AsQueryable();
                query = query.Where(d => d.Category.CategoryId == Id);
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
        public async Task<BaseResponse<FoodListResponse>> TakeAllFoodListByStoreId(int Id)
        {
            var result = new BaseResponse<FoodListResponse> { };
            try
            {
                var query = _datacontext.EN_FoodList.AsQueryable();
                query = query.Where(d => d.UserId == Id);
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
                var data = await _datacontext.EN_CategoryList.FindAsync(Id);
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
        public async Task<BaseResponse<bool>> CreateNewStore(StoreRequest request, HttpPostedFile file)
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
                        _datacontext.EN_Store.Add(province);
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
        public async Task<BaseResponse<bool>> UpdateNewStore(StoreRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.ProvinceId > 0)
                {
                    if (request.FullName == "")
                    {
                        request.FullName = null;
                    }
                    var province = await _datacontext.EN_Store.FindAsync(request.ProvinceId);
                    {
                        province.FullName = request.FullName;
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
                var query = _datacontext.EN_Store.AsQueryable();
                query = query.Where(d => d.Cuisine.CuisineId == filter.CuisineId && d.Status == true);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = FindNearestStores(data.MapTo<StoreResponse>(), filter.latitude, filter.longitude, 40).MapTo<StoreResponse>();
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
        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int Id)
        {
            var result = new BaseResponse<OrderHeaderResponse> { };
            try
            {
                var query = _datacontext.EN_OrderHeader.AsQueryable();
                query = query.Where(d => d.UserId == Id);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = data.MapTo<OrderHeaderResponse>();
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

                //Thong ke chart
                for (int i = 1; i <= 12; i++)
                {
                    double? getStatisticalChartMonth = await _datacontext.EN_OrderHeader
                    .Where(x => x.UserId == request.storeId && x.CreationDate.Value.Month == i)
                    .SumAsync(x => (double?)x.IntoMoney) ?? 0;
                    result.Item.revenueMonth = getStatisticalStoreMonth ?? 0;

                    StatisticalChart chart = new StatisticalChart()
                    {
                        revenueMonth = getStatisticalChartMonth,
                        nameMonth = $"Thang " + i,
                    };
                    result.Item.listChart.Add(chart);
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
                var ApproveStore = await _datacontext.EN_Store.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync();
                var checkAccount = await _datacontext.EN_Account.Where(x => x.UserId == request.UserId).FirstOrDefaultAsync();
                if (checkAccount == null)
                {
                    int newPass = rand.Next(100000, 999999);
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
                            smtp.Credentials = new NetworkCredential("vinhky0167@gmail.com", "zthangqlxnqwuobv");

                            MailMessage mail = new MailMessage();
                            mail.To.Add(request.Email);
                            mail.From = new MailAddress("vinhky0167@gmail.com");
                            mail.Subject = "BẠN ĐÃ ĐƯỢC CẤP MẬT KHẨU BÁN HÀNG";
                            mail.Body = $"Mật khẩu của bạn là: {newPass}.Vui lòng đăng nhập thay đổi mật khẩu mới. Trân trọng !";

                            smtp.Send(mail);
                        }
                    }
                    catch { }
                }
                else
                {
                    ApproveStore.Status = !ApproveStore.Status;
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
            var distances = new Dictionary<StoreResponse, double>();

            // Tính khoảng cách từ điểm đầu tiên đến tất cả các điểm còn lại trong danh sách
            foreach (var store in stores)
            {
                double Lat = Math.Round(store.Latitude, 7);
                double Lon = Math.Round(store.Longitude, 7);
                var distance = Distance(/*lat*/10.370979346894824, /*lng*/105.4306737195795, Lat, Lon);
                store.Distance = distance;
                store.Time = CalculateTime(distance); // Assuming you have a function to calculate time based on distance
                distances[store] = distance;
            }

            // Chọn cửa hàng gần nhất và loại bỏ nó khỏi danh sách
            for (int i = 0; i < count; i++)
            {
                var nearestStore = distances.OrderBy(d => d.Value).FirstOrDefault().Key;
                if (nearestStore != null)
                {
                    nearestStores.Add(nearestStore);
                    distances.Remove(nearestStore);
                }
            }

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
    }
}
