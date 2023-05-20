using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.PushNotification;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Product;
using DaiPhucVinh.Shared.Task;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Tasks
{
    public interface ITaskService
    {
        Task<BaseResponse<TaskResponse>> TakeAllTasks(TaskRequest request);
        Task<BaseResponse<TaskResponse>> TakeAllTasksForChart(TaskRequest request);
        Task<BaseResponse<bool>> CreateTask(TaskRequest request);
        Task<BaseResponse<bool>> UpdateTask(TaskRequest request);
        Task<BaseResponse<TaskResponse>> TakeTaskById(int Id);
        Task<BaseResponse<bool>> RemoveTask(TaskRequest request);
        Task<BaseResponse<ProductByCustomerResponse>> TakeProductByCustomerCode(TaskRequest request);

        #region Mobile
        Task<BaseResponse<TaskMobileResponse>> TakeAllTask_ByUserId(TaskRequest request);
        Task<BaseResponse<TaskResponse>> TakeTaskDetail_ById(TaskRequest request);
        Task<BaseResponse<bool>> TaskCheckIn_ById(TaskRequest request, List<HttpPostedFile> file);
        Task<BaseResponse<bool>> taskEditDetail_Before(TaskRequest request, List<HttpPostedFile> file);
        Task<BaseResponse<bool>> taskEditDetail_After(TaskRequest request, List<HttpPostedFile> file);
        Task<BaseResponse<bool>> taskEditDetail_Document(TaskRequest request, List<HttpPostedFile> file);
        #endregion

    }
    public class TaskService : ITaskService
    {
        private readonly DataContext _datacontext;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly IImageRecordService _imageRecordService;
        private readonly ISetting _settingService;
        private readonly ILogService _logService;
        private readonly IPushMessageService _pushMessageService;
        public TaskService(DataContext datacontext, IUserService userService, IImageService imageService, IImageRecordService imageRecordService, ILogService logService, ISetting settingService, IPushMessageService pushMessageService)
        {
            _datacontext = datacontext;
            _userService = userService;
            _imageService = imageService;
            _imageRecordService = imageRecordService;
            _settingService = settingService;
            _logService = logService;
            _pushMessageService = pushMessageService;
        }
        public async Task<BaseResponse<TaskResponse>> TakeAllTasks(TaskRequest request)
        {
            var result = new BaseResponse<TaskResponse> { };
            try
            {
                DateTime currentdate = DateTime.Now;
                var query = _datacontext.FUV_Tasks.AsQueryable().Where(d => !d.Deleted);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.StartDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.StartDate <= _ToDt);
                }
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term));
                }
                if (request.TaskStatusId > 0)
                {
                    if(request.TaskStatusId == 9999)
                    {
                        query = query.Where(d => d.FUV_TaskStatus.Code != "Done" && d.Deadline.Value < currentdate);
                    }
                    else
                    {
                        query = query.Where(d => d.TaskStatusId == request.TaskStatusId);
                    }
                }
                if (request.TaskTypeId > 0)
                {
                    query = query.Where(d => d.TaskTypeId == request.TaskTypeId);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.CustomerCode.HasValue())
                {
                    query = query.Where(d => d.CustomerCode == request.CustomerCode);
                }
                if (request.AssignUserId > 0)
                {
                    query = query.Where(d => d.AssignUserId == request.AssignUserId);
                }

                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.OrderByDescending(d => d.StartDate).ToListAsync();
                result.Data = data.MapTo<TaskResponse>();

                
                foreach (var task in result.Data)
                {
                    if(task.Deadline < currentdate)
                    {
                        task.IsLate = true;
                    }
                    else
                    {
                        task.IsLate = false;
                    }
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
        public async Task<BaseResponse<TaskResponse>> TakeAllTasksForChart(TaskRequest request)
        {
            var result = new BaseResponse<TaskResponse> { };
            try
            {
                var query = _datacontext.FUV_Tasks.AsQueryable().Where(d => !d.Deleted && d.AssignUserId.HasValue);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.StartDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.StartDate <= _ToDt);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                result.DataCount = await query.CountAsync();
                var data = await query.OrderByDescending(d => d.Id).ToListAsync();
                result.Data = data.MapTo<TaskResponse>();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateTask(TaskRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {
                    if (request.AssignUserId > 0)
                    {
                        // có người thực hiện
                        var task = _datacontext.FUV_Tasks.Add(new FUV_Tasks
                        {
                            Name = request.Name,
                            Description = request.Description,
                            TaskTypeId = request.TaskTypeId,
                            AssignUserId = request.AssignUserId,
                            CustomerCode = request.CustomerCode,
                            CustomerWorkAddress = request.CustomerWorkAddress,
                            CustomerPhone = request.CustomerPhone,
                            Distance = request.Distance,
                            StartDate = request.StartDate,
                            Deadline = request.Deadline,
                            Lat = request.Lat,
                            Long = request.Lng,
                            ItemCode = request.ItemCode,
                            Note_Machine = request.Note_Machine,
                            Note_Suggest = request.Note_Suggest,
                            Note_Result = request.Note_Result,
                            LocationCode = request.LocationCode,
                            TaskStatusId = 2,
                            TaskResultId = null,
                            CreatedAt = DateTime.Now,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                            IsDone = false,
                            Deleted = false,
                        });
                        await _datacontext.SaveChangesAsync();

                        #region push notification
                        var mobileToken = await _datacontext.FUV_MobileDevice.FirstOrDefaultAsync(x => x.Active.HasValue && x.Active.Value
                                                                                    && x.UserId == task.AssignUserId.Value);
                        var listDeviceToken = new List<string>();

                        if (mobileToken != null)
                        {
                            var user = await _datacontext.Users.FirstOrDefaultAsync(x => x.Id == mobileToken.UserId);
                            if (user != null)
                            {
                                if (user.RoleSystem == "KyThuatVien")
                                {
                                    listDeviceToken.Add(mobileToken.DeviceToken);
                                }
                            }
                        }
                        await _pushMessageService.Push("Công việc mới", "Bạn có một công việc mới", listDeviceToken);
                        #endregion
                    }
                    else
                    {
                        //không có người thực hiện
                        var task = _datacontext.FUV_Tasks.Add(new FUV_Tasks
                        {
                            Name = request.Name,
                            Description = request.Description,
                            TaskTypeId = request.TaskTypeId,
                            AssignUserId = request.AssignUserId,
                            CustomerCode = request.CustomerCode,
                            CustomerWorkAddress = request.CustomerWorkAddress,
                            CustomerPhone = request.CustomerPhone,
                            Distance = request.Distance,
                            StartDate = request.StartDate,
                            ItemCode = request.ItemCode,
                            Deadline = request.Deadline,
                            Lat = request.Lat,
                            Long = request.Lng,
                            Note_Machine = request.Note_Machine,
                            Note_Suggest = request.Note_Suggest,
                            Note_Result = request.Note_Result,
                            LocationCode = request.LocationCode,
                            TaskStatusId = 1,
                            TaskResultId = null,
                            CreatedAt = DateTime.Now,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                            UpdatedAt = DateTime.Now,
                            UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                            IsDone = false,
                            Deleted = false,
                        });
                        await _datacontext.SaveChangesAsync();
                    }
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
        public async Task<BaseResponse<bool>> UpdateTask(TaskRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    if (request.AssignUserId > 0) // có người thực hiện
                    {
                        
                        if (request.TaskStatusId == 1) //trạng thái khởi tạo => đã giao
                        {
                            var task = await _datacontext.FUV_Tasks.SingleOrDefaultAsync(x => x.Id == request.Id);
                            task.Name = request.Name;
                            task.Description = request.Description;
                            task.TaskTypeId = request.TaskTypeId;
                            task.AssignUserId = request.AssignUserId;
                            task.CustomerCode = request.CustomerCode;
                            task.CustomerWorkAddress = request.CustomerWorkAddress;
                            task.ItemCode = request.ItemCode;
                            task.CustomerPhone = request.CustomerPhone;
                            task.Distance = request.Distance;
                            task.StartDate = request.StartDate;
                            task.Deadline = request.Deadline;
                            task.Lat = request.Lat;
                            task.Long = request.Lng;
                            task.LocationCode = request.LocationCode;
                            task.TaskStatusId = 2; //2 là trạng thái đã giao
                            task.UpdatedAt = DateTime.Now;
                            task.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                        }
                        else //trạng thái khác khởi tạo => không thay đổi trạng thái công việc
                        {
                            var task = await _datacontext.FUV_Tasks.SingleOrDefaultAsync(x => x.Id == request.Id);
                            task.Name = request.Name;
                            task.Description = request.Description;
                            task.TaskTypeId = request.TaskTypeId;
                            task.AssignUserId = request.AssignUserId;
                            task.CustomerCode = request.CustomerCode;
                            task.CustomerWorkAddress = request.CustomerWorkAddress;
                            task.CustomerPhone = request.CustomerPhone;
                            task.ItemCode = request.ItemCode;
                            task.Distance = request.Distance;
                            task.StartDate = request.StartDate;
                            task.Deadline = request.Deadline;
                            task.Lat = request.Lat;
                            task.Long = request.Lng;
                            task.LocationCode = request.LocationCode;
                            task.UpdatedAt = DateTime.Now;
                            task.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                        }
                    }
                    else
                    {
                        //không có người thực hiện
                        var task = await _datacontext.FUV_Tasks.SingleOrDefaultAsync(x => x.Id == request.Id);
                        task.Name = request.Name;
                        task.Description = request.Description;
                        task.TaskTypeId = request.TaskTypeId;
                        task.AssignUserId = request.AssignUserId;
                        task.CustomerCode = request.CustomerCode;
                        task.CustomerWorkAddress = request.CustomerWorkAddress;
                        task.CustomerPhone = request.CustomerPhone;
                        task.ItemCode = request.ItemCode;
                        task.Distance = request.Distance;
                        task.StartDate = request.StartDate;
                        task.Deadline = request.Deadline;
                        task.Lat = request.Lat;
                        task.Long = request.Lng;
                        task.LocationCode = request.LocationCode;
                        task.TaskStatusId = 1; //1 là trạng thái khởi tạo công việc
                        task.UpdatedAt = DateTime.Now;
                        task.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<TaskResponse>> TakeTaskById(int Id)
        {
            var result = new BaseResponse<TaskResponse> { };
            try
            {
                var data = await _datacontext.FUV_Tasks.FindAsync(Id);
                result.Item = data.MapTo<TaskResponse>();

                var _user = await _userService.TakeUserByUserName(result.Item.CreatedBy);
                result.Item.CreatedBy = _user.Item.DisplayName;

                if (result.Item.LocationImages.HasValue())
                {
                    var IdList = result.Item?.LocationImages;
                    result.Item.LocationImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.BeforeProcessImages.HasValue())
                {
                    var IdList = result.Item?.BeforeProcessImages;
                    result.Item.BeforeProcessImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.AfterProcessImages.HasValue())
                {
                    var IdList = result.Item?.AfterProcessImages;
                    result.Item.AfterProcessImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.DocumentImages.HasValue())
                {
                    var IdList = result.Item?.DocumentImages;
                    result.Item.DocumentImageList = await _imageRecordService.GetImageList(IdList);
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
        public async Task<BaseResponse<bool>> RemoveTask(TaskRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var task = await _datacontext.FUV_Tasks.SingleOrDefaultAsync(x => x.Id == request.Id);
                task.Deleted = true;
                task.UpdatedBy = TokenHelper.CurrentIdentity().DisplayName;
                task.UpdatedAt = DateTime.Now;
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
        public async Task<BaseResponse<ProductByCustomerResponse>> TakeProductByCustomerCode(TaskRequest request)
        {
            var result = new BaseResponse<ProductByCustomerResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDong_ChiTiets.AsQueryable();
                if (request == null)
                {
                    result.Message = "Không lấy được dữ liệu";
                    result.Success = false;
                    return result;
                }

                if (!string.IsNullOrEmpty(request.CustomerCode))
                {
                    query = query.Where(x => x.WMS_HopDong.KhachHang_Code.Equals(request.CustomerCode)
                                                    && x.WMS_HopDong.IsDaKy.HasValue && x.WMS_HopDong.IsDaKy.Value
                                                    && x.WMS_Item.IsDelete.HasValue && !x.WMS_Item.IsDelete.Value);
                    var dataItem = query.GroupBy(x => new { x.ItemCode, x.WMS_Item.Name });
                    var data2 = await dataItem.Select(s => new ProductByCustomerResponse
                    {
                        ItemCode = s.Key.ItemCode,
                        ItemName = s.Key.Name,
                    }).ToListAsync();
                    result.Data = data2.MapTo<ProductByCustomerResponse>();
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

        #region [Mobile]
        public async Task<BaseResponse<TaskMobileResponse>> TakeAllTask_ByUserId(TaskRequest request)
        {
            var result = new BaseResponse<TaskMobileResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Tasks.AsQueryable().Where(s => !s.Deleted && s.AssignUserId == UserId);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.StartDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.StartDate <= _ToDt);
                }
                query = query.OrderBy(d => d.StartDate).Take(5);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<TaskMobileResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<TaskResponse>> TakeTaskDetail_ById(TaskRequest request)
        {
            var result = new BaseResponse<TaskResponse> { };
            try
            {
                var query = await _datacontext.FUV_Tasks.FindAsync(request.Id);
                result.Item = query.MapTo<TaskResponse>();

                if (result.Item.LocationImages.HasValue())
                {
                    var IdList = result.Item?.LocationImages;
                    result.Item.LocationImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.BeforeProcessImages.HasValue())
                {
                    var IdList = result.Item?.BeforeProcessImages;
                    result.Item.BeforeProcessImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.AfterProcessImages.HasValue())
                {
                    var IdList = result.Item?.AfterProcessImages;
                    result.Item.AfterProcessImageList = await _imageRecordService.GetImageList(IdList);
                }

                if (result.Item.DocumentImages.HasValue())
                {
                    var IdList = result.Item?.DocumentImages;
                    result.Item.DocumentImageList = await _imageRecordService.GetImageList(IdList);
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
        public async Task<BaseResponse<bool>> TaskCheckIn_ById(TaskRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var timestart = request.TimeToStart.Value.AddHours(7);

                var UserId = TokenHelper.CurrentIdentity().UserId;
                var entity = await _datacontext.FUV_Tasks.FindAsync(request.Id);
                entity.TimeToStart = timestart;
                entity.TimeToLocation = DateTime.Now;
                entity.CheckToLocation = true;
                entity.TaskStatusId = 3;
                if (file.Count > 0)
                {
                    List<int> imagesListId = entity.LocationImages != null && entity.LocationImages.Contains(",")
                            ? entity.LocationImages.Split(',').Select(x => int.Parse(x)).ToList()
                            : new List<int>();
                    foreach (var image in file)
                    {
                        if(image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                    }
                    entity.LocationImages = string.Join(",", imagesListId);
                }
                var task = _datacontext.FUV_Checkins.Add(new FUV_Checkin
                {
                    UserId = UserId,
                    Long = request.Long,
                    Lat = request.Lat,
                    TimeCheckin = DateTime.Now,
                    Deleted = false,
                });

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
        public async Task<BaseResponse<bool>> taskEditDetail_Before(TaskRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var entity = await _datacontext.FUV_Tasks.FindAsync(request.Id);
                entity.Note_Machine = request.Note_Machine;
                entity.Note_Suggest = request.Note_Suggest;
                entity.Note_Result = request.Note_Result;
                entity.TaskStatusId = 4;
                entity.TaskResultId = request.TaskResultId;
                entity.TaskFinishAt = DateTime.Now;
                if (file.Count > 0)
                {
                    List<int> imagesListId = new List<int>();
                    foreach (var image in file)
                    {
                        if(image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                        
                    }
                    entity.BeforeProcessImages = string.Join(",", imagesListId);
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
        public async Task<BaseResponse<bool>> taskEditDetail_After(TaskRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var entity = await _datacontext.FUV_Tasks.FindAsync(request.Id);
                if (file.Count > 0)
                {
                    List<int> imagesListId = new List<int>();
                    foreach (var image in file)
                    {
                        if(image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                    }
                    entity.AfterProcessImages = string.Join(",", imagesListId);
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
        public async Task<BaseResponse<bool>> taskEditDetail_Document(TaskRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var entity = await _datacontext.FUV_Tasks.FindAsync(request.Id);
                if (file.Count > 0)
                {
                    List<int> imagesListId = new List<int>();
                    foreach (var image in file)
                    {
                        if (image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                    }
                    entity.DocumentImages = string.Join(",", imagesListId);
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
        #endregion

    }
}