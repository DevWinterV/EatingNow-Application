using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Services.MainServices.Tasks;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Customer;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Employee
{
    public interface IEmployeeService
    {
        Task<BaseResponse<EmployeeResponse>> TakeAllsEmployee(EmployeeRequest request);
        Task<BaseResponse<EmployeeResponse>> SearchEmployee(string EmployeeCode);
        Task<BaseResponse<EmployeeResponse>> TakeEmployee_ByUserLogin(EmployeeRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeNotificationReceiver(NotificationGroupRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeEmployeeNotReceiveNotification(NotificationGroupRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeEmployeeNotReceiveNotificationPageCreate(NotificationGroupRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeAllEmployeeCode(CustomerRequest request);
        Task<BaseResponse<bool>> UpdateEmployee(EmployeeRequest request);

        #region[Mobile]
        Task<BaseResponse<EmployeeResponse>> ProfileTakeDetail_ByUserLogin(EmployeeRequest request);
        Task<BaseResponse<bool>> ProfileEdit_ByUserLogin(EmployeeRequest request, HttpPostedFile file);
        #endregion
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly IImageService _imageService;
        private readonly IImageRecordService _imageRecordService;
        private readonly ISetting _settingService;
        private readonly ITaskService _taskService;
        public EmployeeService(DataContext datacontext, ILogService logService, IImageService imageService, IImageRecordService imageRecordService, ISetting settingService, ITaskService taskService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _imageService = imageService;
            _imageRecordService = imageRecordService;
            _settingService = settingService;
            _taskService = taskService;
        }
        public async Task<BaseResponse<EmployeeResponse>> TakeAllsEmployee(EmployeeRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var query = _datacontext.WMS_Employees.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.FullName.Contains(request.Term));
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<EmployeeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> SearchEmployee(string EmployeeCode)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var query = await _datacontext.WMS_Employees.SingleOrDefaultAsync(x => x.EmployeeCode == EmployeeCode);
                result.Item = query.MapTo<EmployeeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> TakeEmployee_ByUserLogin(EmployeeRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var userName = TokenHelper.CurrentIdentity().UserName;
                var query = await _datacontext.WMS_Employees.SingleOrDefaultAsync(x => x.UserLogin == userName);
                if (query != null)
                {
                    result.Item = query.MapTo<EmployeeResponse>();
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
        public async Task<BaseResponse<EmployeeResponse>> TakeNotificationReceiver(NotificationGroupRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                if (!request.UserIds.IsNullOrEmpty())
                {
                    List<string> listIds = new List<string> { };
                    listIds = request.UserIds.Split(',')?.ToList();
                    var employeeReceiver = _datacontext.WMS_Employees.Where(d => listIds.Contains(d.EmployeeCode));
                    result.DataCount = await employeeReceiver.CountAsync();
                    employeeReceiver = employeeReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    var data = await employeeReceiver.ToListAsync();
                    result.Data = data.MapTo<EmployeeResponse>();
                    result.Success = true;
                }
                else
                {
                    List<int> listIds = new List<int> { };

                    var employeeReceiver = _datacontext.WMS_Employees.Where(d => listIds.Contains(0));
                    result.DataCount = await employeeReceiver.CountAsync();
                    employeeReceiver = employeeReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    var data = await employeeReceiver.ToListAsync();
                    result.Data = data.MapTo<EmployeeResponse>();
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
        public async Task<BaseResponse<EmployeeResponse>> TakeEmployeeNotReceiveNotification(NotificationGroupRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                if (!request.UserIds.IsNullOrEmpty())
                {
                    List<string> listIds = new List<string> { };
                    listIds = request.UserIds.Split(',')?.ToList();
                    var employeeNotReceiver = _datacontext.WMS_Employees.Where(d => !listIds.Contains(d.EmployeeCode));
                    result.DataCount = await employeeNotReceiver.CountAsync();
                    employeeNotReceiver = employeeNotReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    var data = await employeeNotReceiver.ToListAsync();
                    result.Data = data.MapTo<EmployeeResponse>();
                    result.Success = true;
                }
                else
                {
                    var query = _datacontext.WMS_Employees.AsQueryable();
                    result.DataCount = await query.CountAsync();
                    query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EmployeeResponse>();
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
        public async Task<BaseResponse<EmployeeResponse>> TakeEmployeeNotReceiveNotificationPageCreate(NotificationGroupRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                if (!request.UserIds.IsNullOrEmpty())
                {
                    if (request.dataItemCheck.Count > 0)
                    {
                        List<string> listIds = new List<string> { };
                        foreach (var user in request.dataItemCheck)
                        {
                            listIds.Add(user.EmployeeCode);
                        }
                        var employeeNotReceiver = _datacontext.WMS_Employees.Where(d => !listIds.Contains(d.EmployeeCode));

                        result.DataCount = await employeeNotReceiver.CountAsync();
                        employeeNotReceiver = employeeNotReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        var data = await employeeNotReceiver.ToListAsync();
                        result.Data = data.MapTo<EmployeeResponse>();
                        result.Success = true;
                    }
                    else
                    {
                        List<string> listIds = request.UserIds.Split(',')?.ToList();
                        var employeeNotReceiver = _datacontext.WMS_Employees.Where(d => !listIds.Contains(d.EmployeeCode));
                        result.DataCount = await employeeNotReceiver.CountAsync();
                        employeeNotReceiver = employeeNotReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        var data = await employeeNotReceiver.ToListAsync();
                        result.Data = data.MapTo<EmployeeResponse>();
                        result.Success = true;
                    }

                }
                else
                {
                    if (request.dataItemCheck.Count > 0)
                    {
                        List<int> listIds = new List<int> { };
                        foreach (var user in request.dataItemCheck)
                        {
                            listIds.Add(user.Id);
                        }
                        var employeeNotReceiver = _datacontext.WMS_Employees.Where(d => !listIds.Contains(d.Id));

                        result.DataCount = await employeeNotReceiver.CountAsync();
                        employeeNotReceiver = employeeNotReceiver.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        var data = await employeeNotReceiver.ToListAsync();
                        result.Data = data.MapTo<EmployeeResponse>();
                        result.Success = true;
                    }
                    else
                    {
                        var query = _datacontext.WMS_Employees.AsQueryable();
                        result.DataCount = await query.CountAsync();
                        query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                        var data = await query.ToListAsync();
                        result.Data = data.MapTo<EmployeeResponse>();
                        result.Success = true;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> TakeAllEmployeeCode(CustomerRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var query = _datacontext.WMS_Employees.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.FullName.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.FullName);
                var data = await query.Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync();
                result.Data = data.MapTo<EmployeeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateEmployee(EmployeeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = _datacontext.WMS_Employees.AsQueryable().FirstOrDefault(x => x.EmployeeCode == request.EmployeeCode);
                query.FullName = request.FullName;
                query.Tel = request.Tel;
                query.Address = request.Address;
                query.Email = request.Email;
                query.LocationCode = request.LocationCode;
                _datacontext.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        #region[APP]
        public async Task<BaseResponse<EmployeeResponse>> ProfileTakeDetail_ByUserLogin(EmployeeRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var UserLogin = TokenHelper.CurrentIdentity().UserName;
                var query = await _datacontext.WMS_Employees.SingleOrDefaultAsync(x => x.UserLogin == UserLogin);
                result.Item = query.MapTo<EmployeeResponse>();
                if (result.Item.ImageRecordId.HasValue())
                {
                    var IdImage = result.Item?.ImageRecordId;
                    result.Item.Avarta = await _imageRecordService.GetImageList(IdImage);
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ProfileEdit_ByUserLogin(EmployeeRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserLogin = TokenHelper.CurrentIdentity().UserName;
                var entity = await _datacontext.WMS_Employees.SingleOrDefaultAsync(d => d.UserLogin == UserLogin);
                entity.FullName = request.FullName;
                entity.Tel = request.PhoneNo;
                entity.Address = request.Address;
                if (file != null)
                {
                    HttpPostedFile file_Img = null;
                    if (_imageService.CheckImageFileType(file.FileName))
                    {
                        file_Img = file;
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file_Img.InputStream.CopyTo(ms);
                        var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                        entity.ImageRecordId = Images.Image.Id;
                    }
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        #endregion
    }
}
