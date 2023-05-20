using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Cuisine;
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

namespace DaiPhucVinh.Services.MainServices.Cuisine
{
    public interface ICuisineService
    {
        string HostAddress { get; }
        Task<BaseResponse<CuisineResponse>> TakeAllCuisine(CuisineRequest request);
        Task<BaseResponse<bool>> CreateNewCuisine(CuisineRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdateNewCuisineHaveImage(CuisineRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdateNewCuisineNotImage(CuisineRequest request);
        Task<BaseResponse<bool>> DeleteCuisine(CuisineRequest request);
        Task<BaseResponse<CuisineResponse>> SearchCuisine(string nameCuisine);
        Task<BaseResponse<CuisineResponse>> TakeCuisineById(int Id);
    }
    public class CuisineService : ICuisineService
    {
        
        const string SubPath = "\\uploads\\DaiPhucVinh\\Image";
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public CuisineService(DataContext datacontext, ILogService logService, ICommonService commonService, ILogger logger, ISettingService settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
            _settingService = settingService;
            _logger = logger;
        }


        public async Task<BaseResponse<CuisineResponse>> TakeAllCuisine(CuisineRequest request)
        {
            var result = new BaseResponse<CuisineResponse> { };
            try
            {
                var query = _datacontext.EN_Cuisine.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                var resultList = data.MapTo<CuisineResponse>();
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
        public async Task<BaseResponse<CuisineResponse>> TakeCuisineById(int Id)
        {
            var result = new BaseResponse<CuisineResponse> { };
            try
            {
                var data = await _datacontext.EN_Cuisine.FindAsync(Id);
                result.Item = data.MapTo<CuisineResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNewCuisine(CuisineRequest request, HttpPostedFile file)
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

                    if (request.CuisineId == 0)
                    {
                        if (request.Name == "")
                        {
                            request.Name = null;
                        }
                        var cuisine = new EN_Cuisine
                        {
                            Name = request.Name,
                            AbsoluteImage = HostAddress + GenAbsolutePath(relativePath),    
                        };

                        _datacontext.EN_Cuisine.Add(cuisine);
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
        public async Task<BaseResponse<bool>> UpdateNewCuisineHaveImage(CuisineRequest request, HttpPostedFile file)
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

                    if (request.CuisineId > 0)
                    {
                        if (request.Name == "")
                        {
                            request.Name = null;
                        }
                        var cuisine = await _datacontext.EN_Cuisine.FindAsync(request.CuisineId);
                        {
                            cuisine.Name = request.Name;
                            cuisine.AbsoluteImage = HostAddress + GenAbsolutePath(relativePath);
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
        public async Task<BaseResponse<bool>> UpdateNewCuisineNotImage(CuisineRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                    if (request.CuisineId > 0)
                    {
                        if (request.Name == "")
                        {
                            request.Name = null;
                        }
                        var cuisine = await _datacontext.EN_Cuisine.FindAsync(request.CuisineId);
                        {
                            cuisine.Name = request.Name;
                            cuisine.AbsoluteImage = request.AbsoluteImage;
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
        public async Task<BaseResponse<bool>> DeleteCuisine(CuisineRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var cuisine = await _datacontext.EN_Cuisine.FirstOrDefaultAsync(x => x.CuisineId == request.CuisineId);
                _datacontext.EN_Cuisine.Remove(cuisine);
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
        public async Task<BaseResponse<CuisineResponse>> SearchCuisine(string cuisineName)
        {
            var result = new BaseResponse<CuisineResponse> { };
            try
            {
                var query = await _datacontext.EN_Cuisine.SingleOrDefaultAsync(x => x.Name == cuisineName);
                result.Item = query.MapTo<CuisineResponse>();
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
    