using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
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
    }
}
